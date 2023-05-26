# import speech_recognition as sr
# import pyttsx3
import utils.DataCleanser as dc


# Setup voice engine
# engine = pyttsx3.init('sapi5')
# voices = engine.getProperty('voices')
# engine.setProperty('utils', 'voices[0].id')


# Speaks a string of text
# def speak(text):
#     engine.say(text)
#     engine.runAndWait()


# Listens to mic input
def listen(model, do_voice_input, use_encoded_responses):
    model_response = None

    # if do_voice_input:
    #     r = sr.Recognizer()
    #     with sr.Microphone() as source:
    #         print("Listening...")
    #         audio = r.listen(source)

    #         try:
    #             statement = r.recognize_google(audio, language='en-in')
    #             print(f"User: {statement}\n")
    #             model_response = model.evaluate(statement)

    #         except Exception as e:
    #             model_response = "Pardon me, please say that again"
    # else:
    statement = input("User: ")

    evaluation = model.evaluate(statement)
    model_response = dc.encode_response(evaluation) if use_encoded_responses else evaluation

    # Print bot response and speak back to user
    print("Bot: " + model_response + '\n')
    # speak(model_response)
