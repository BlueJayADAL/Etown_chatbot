import socket
import speech_recognition as sr
from gtts import gTTS
import os
import numpy as np
import struct
import wave
from pydub import AudioSegment
from os import path
import json
import requests
import threading
import sys

sample_rate = 44100

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
    



def listen( do_voice_input, use_encoded_responses, audiodata):
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

    
    except Exception as e:
        return "I'm sorry I had trouble understanding your question could you ask again?"
    except sr.UnknownValueError:
        print("Unable to recognize speech.")
    except sr.RequestError as e:
        print("Error occurred during speech recognition: {0}".format(e))
    

    chatbot = Chatbot()
    model_response = chatbot.get_chatbot_response(recognized_text)

    if model_response==None or model_response=="" or model_response==" ":
        model_response = "I'm sorry I couldn't understand the question"

    print("Bot: " + model_response + '\n')

    return model_response
# Socket configuration
HOST = "172.16.80.112"
PORT = 12345
 
    
class Chatbot:
    def __init__(self):
        self.client = requests.Session()

    def get_chatbot_response(self, question):
        request_body = {
            "question": question,
            "history": []  # include chat history if needed
        }
        json_content = json.dumps(request_body)
        headers = {"Content-Type": "application/json"}

        response = self.client.post(
            #Put the link to the Web hosted server here
            "https://error-54odydxkuq-uc.a.run.app/api/chat", data=json_content, headers=headers)
        response_text = response.text
        response_object = json.loads(response_text)

        return response_object["text"]


timeout_duration = 15  # 15 Seconds

# Set up the socket and start listening for audio data

def main():
    while(True):
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.bind((HOST, PORT))

            s.listen(1)
            print(f"Listening on {HOST}:{PORT}...")

            client_socket, addr = s.accept()
            print(f"Connection from {addr} established")
            timer = threading.Timer(timeout_duration, timeout_handler)
            timer.start()
            audio_data = None
            data=None
            audio_data = b""   
            while True:
                data = client_socket.recv(1024)
                if not data:
                    break
                audio_data += data
            #print(audio_data)
            timer.cancel()
            timer = threading.Timer(timeout_duration, timeout_handler)
            timer.start()
            text= listen(True,False,audio_data)
            TexttoSpeech(text)
        #   text = convert_audio_to_text(audio_data)

            print(f"Received audio: {text}")
        client_socket.close()
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as server_socket:
            server_socket.bind((HOST, PORT))
            server_socket.listen(1)
            timer.cancel()
            timer = threading.Timer(timeout_duration, timeout_handler)
            timer.start()
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
        timer.cancel()

def timeout_handler():
    raise TimeoutError("Script timed out")

if True:
    try:
            main()
    except TimeoutError:
            print("Script timed out. Restarting...")
            # Restart the script by running the Python interpreter again
            # Note: Make sure to pass the same arguments as the initial script
            python = sys.executable
            sys.stdout.flush()
            sys.stderr.flush()
            sys.exit(os.execvp(python, [python] + sys.argv))