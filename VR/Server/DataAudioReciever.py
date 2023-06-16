import socket
from pocketsphinx import LiveSpeech
import speech_recognition as sr
from gtts import gTTS
import DataCleanser as dc
import openai
import os
from dotenv import load_dotenv
load_dotenv()
import binascii
import numpy as np
import struct
import wave
import pyaudio
from pydub import AudioSegment
from os import path
sample_rate = 44100
openai.api_key = os.getenv("OpenAI_API_KEY")
#Setup voice engine


#Speaks a string of text

def TexttoSpeech(audiofile):
    # The text that you want to convert to audio
    mytext = audiofile
  
# Language in which you want to convert
    language = 'en'
  
    # Passing the text and language to the engine, 
    # here we have marked slow=False. Which tells 
    # the module that the converted audio should 
    #    have a high speed
    myobj = gTTS(text=mytext, lang=language, slow=False)
  
    # Saving the converted audio in a mp3 file named
    # welcome 
    myobj.save("sendfile.mp3")
    convert_mp3_to_wav("sendfile.mp3", "sendfile.wav")
    # Playing the converted file
    return 
    # Listens to mic input
def convert_mp3_to_wav(src, dst):
    sound = AudioSegment.from_mp3(src)
    sound.export(dst, format="wav")
    



def listen(model1, do_voice_input, use_encoded_responses, audiodata):
    model_response = None
    
    pcm_samples = np.frombuffer(audiodata, dtype=np.int16)

    # Convert PCM samples to float samples
    float_samples = pcm_samples.astype(np.float32) / 32767.0
    pcm_samples = [int(sample * 32767) for sample in float_samples]
    pcm_data = struct.pack('<' + 'h' * len(pcm_samples), *pcm_samples)
    recognizer = sr.Recognizer()
  
    # Write PCM data to a temporary WAV file
    with wave.open('temp.wav', 'wb') as wav_file:
        wav_file.setnchannels(1)
        wav_file.setsampwidth(2)
        wav_file.setframerate(sample_rate)
        wav_file.writeframes(pcm_data)

    try:
        with sr.AudioFile('temp.wav') as audio_file:
            audio12 = recognizer.record(audio_file)

            recognized_text = ''+recognizer.recognize_google(audio12, show_all=False,with_confidence=False)
            print(recognized_text)
        #first_transcript = recognized_text['alternative'][0]['transcript']
        #print(first_transcript)
    
    except Exception as e:
        return "I'm sorry I had trouble understanding your question could you ask again?"
    except sr.UnknownValueError:
        print("Unable to recognize speech.")
    except sr.RequestError as e:
        print("Error occurred during speech recognition: {0}".format(e))
    
 #   print(statement)
    
    evaluation = model1.evaluate(recognized_text,recognized_text)
    model_response = dc.encode_response(evaluation) if use_encoded_responses else evaluation
    if model_response==None or model_response=="" or model_response==" ":
        model_response = "I'm sorry I couldn't understand the question"
    #Print bot response and speak back to user
    print("Bot: " + model_response + '\n')
    # speak(model_response)
    return model_response
# Socket configuration
HOST = "172.16.80.112"
PORT = 12345
 
# Configure PocketSphinx
config = {
    'verbose': False,
    'audio_device': 'plughw:1,0',  # Replace with your audio device
    'buffer_size':2048
}

#def convert_audio_to_text(audio_data):
#    s

# Set up the socket and start listening for audio data


class GPT3:
    doc_data, type = None, None

    def __init__(self, model_type, doc_data):
        self.type = model_type
        self.doc_data = doc_data

    def evaluate(self, text):
        print(text)
        # Initialize model based on passed parameters
        if True:
            response = openai.Completion.create(
                engine="davinci",
                prompt='Q: ' + text + "\nA: ",
                temperature=0.3,
                max_tokens=100,
                top_p=1,
                frequency_penalty=0,
                presence_penalty=0.3,
                stop=['\n', '<|endoftext|>']
            )
            response = str(response['choices'][0]['text'])

            # Remove question marks at beginning of text response if they exist
            if response[:2] == '??':
                response = response[2:]
            return response
        elif self.type == 'ETOWN':
            response = openai.Answer.create(
                search_model="ada",
                model='curie',
                question=text,
                documents=self.doc_data,
                examples_context="When was etown founded?",
                examples=[["Elizabethtown College was founded in 1899.", "Etown was founded in 1899."]],
                max_tokens=30,
                stop=['\n', '<|endoftext|>'],
            )
            return response['answers'][0]
# Set up the socket and start listening for audio data
model=GPT3
while(True):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((HOST, PORT))

        s.listen(1)
        print(f"Listening on {HOST}:{PORT}...")

        client_socket, addr = s.accept()
        print(f"Connection from {addr} established")
        audio_data = None
        data=None
        audio_data = b""   
        while True:
            data = client_socket.recv(1024)
            if not data:
                break
            audio_data += data
        #print(audio_data)
        text= listen(model,True,False,audio_data)
        TexttoSpeech(text)
    #   text = convert_audio_to_text(audio_data)

        print(f"Received audio: {text}")
    client_socket.close()
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as server_socket:
        server_socket.bind((HOST, PORT))
        server_socket.listen(1)
        print(f"Listening on {HOST}:{PORT}...")
        file_data = None
        client_socket, addr = server_socket.accept()
        print(f"Connection from {addr} established")
        with open("sendfile.wav", 'rb') as file:
        # Read the contents of the file
            file_data = file.read()
        file_size = len(file_data)
        

        print(str(file_size))    
        # Send the file data to the client
        client_socket.sendall(file_data)
    client_socket.close()