from happytransformer import HappyGeneration
from happytransformer import GENSettings
from transformers import pipeline


class GPTNeo:
    model, settings, type = None, None, None

    def __init__(self, model_type):
        self.type = model_type

        # Initialize model based on passed parameters
        if model_type == '125M':
            self.model = HappyGeneration(model_type='GPT-NEO', model_name='EleutherAI/gpt-neo-125M')
            self.settings = GENSettings(do_sample=True, top_k=50, max_length=30, min_length=10)
        elif model_type == '2.7B':
            self.model = pipeline('text-generation', model='EleutherAI/gpt-neo-2.7B')

    def evaluate(self, text):
        if self.type == '125M':
            return self.model.generate_text(text, args=self.settings).text.split('\n').split('\n')[0].split('.')[0]
        elif self.type == '2.7B':
            return self.model(text, do_sample=True, max_length=30, min_length=10)[0]['generated_text'].split('\n')[0].split('.')[0]

        return None

