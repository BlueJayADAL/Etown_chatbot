import openai
import os
from dotenv import load_dotenv

# Load .env file
load_dotenv()

openai.api_key = os.getenv("OpenAI_API_KEY")

# Access environment variables
value1 = os.getenv("KEY1")

class GPT3:
    doc_data, type = None, None

    def __init__(self, model_type, doc_data):
        self.type = model_type
        self.doc_data = doc_data

    def evaluate(self, text):

        # Initialize model based on passed parameters
        if self.type == 'GPT3':
            response = openai.Completion.create(
                engine="davinci",
                prompt='Q: ' + text + "\nA: ",
                temperature=0.3,
                max_tokens=30,
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
