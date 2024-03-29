import socket
import speech_recognition as sr
from gtts import gTTS
import numpy as np
import struct
import wave
from pydub import AudioSegment
from os import path
import json
import requests
import psutil

sample_rate = 44100



def TexttoSpeech(audiofile):

    mytext = audiofile
  

    language = 'en'
  

    myobj = gTTS(text=mytext, lang=language, slow=False)
  
 
    myobj.save("sendfile.mp3")
    convert_mp3_to_wav("sendfile.mp3", "sendfile.wav")

    return 

def convert_mp3_to_wav(src, dst):
    sound = AudioSegment.from_mp3(src)
    sound.export(dst, format="wav")
    



def listen( do_voice_input, use_encoded_responses, audiodata):
    model_response = None
    
    pcm_samples = np.frombuffer(audiodata, dtype=np.int16)

    float_samples = pcm_samples.astype(np.float32) / 32767.0
    pcm_samples = [int(sample * 32767) for sample in float_samples]
    pcm_data = struct.pack('<' + 'h' * len(pcm_samples), *pcm_samples)
    recognizer = sr.Recognizer()
  
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


def get_outward_ipv4():
    try:
        response = requests.get('https://api.ipify.org')
        if response.status_code == 200:
            return response.text
        else:
            print(f"Failed to retrieve IP address. Status Code: {response.status_code}")
    except requests.RequestException as e:
        print(f"An error occurred while fetching the IP address: {e}")

def get_local_ipv4():
    with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as s:
        try:
            s.connect(("8.8.8.8", 80))
            local_ip = s.getsockname()[0]
        except socket.error:
            local_ip = "127.0.0.1" 
    return local_ip

ipv4 = get_outward_ipv4()
HOST = get_local_ipv4()
PORT = 45250
PORT2 = 45251

class Chatbot:
    def __init__(self):
        self.client = requests.Session()

    def get_chatbot_response(self, question):
        request_body = {
            "question": question,
            "history": [] 
        }
        json_content = json.dumps(request_body)
        headers = {"Content-Type": "application/json"}

        response = self.client.post(

            "https://error-54odydxkuq-uc.a.run.app/api/chat", data=json_content, headers=headers)
        response_text = response.text
        response_object = json.loads(response_text)

        return response_object["text"]



def main():
    print(f"Hello! This machine is currently running on IPv4 address {HOST} on ports {PORT} and {PORT2}. If you are trying to configure the VR game, use the panels behind you when you spawn and input those three numbers in the order printed above. Note that this will only work if you are connecting from a headset on the same network. If you are trying to connect over the internet, it will require inputting the outward IP address ({ipv4}) in the VR game, as well as needing the ports as listed above to be forwarded to the local IP address of this machine: {HOST}, for both receiving and sending data.")
    while(True):
        terminate_existing_process(PORT)
        terminate_existing_process(PORT2)
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:

            s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
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

            text= listen(True,False,audio_data)
            TexttoSpeech(text)

            print(f"Received audio: {text}")
            client_socket.close()
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as server_socket:
            server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
            server_socket.bind((HOST, PORT2))
            server_socket.listen(1)

            print(f"Listening on {HOST}:{PORT2}...")
            file_data = None
            client_socket, addr = server_socket.accept()
            print(f"Connection from {addr} established")
            with open("sendfile.wav", 'rb') as file:
                file_data = file.read()
            file_size = len(file_data)
            

            print(str(file_size))    
            client_socket.sendall(file_data)
            client_socket.close()

            
def terminate_existing_process(port):
    for process in psutil.process_iter():
        try:
            connections = process.connections()
            for conn in connections:
                if conn.laddr.port == port:
                    process.terminate()
                    print(f"Terminated process with PID: {process.pid}")
        except (psutil.NoSuchProcess, psutil.AccessDenied, psutil.ZombieProcess):
            pass
        


main()