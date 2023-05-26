import sys
from models.GPT3 import GPT3
# from models.GPTNeo import GPTNeo
# from models.LDA import LDA
# from models.Siamese import Siamese
from utils.InputHandler import listen
# from utils import DataCleanser as dc
# import pickle
# import pandas as pd

DO_VOICE_INPUT = False


def main():
    args = sys.argv[1:]

    if len(args) == 2 and (args[0] == '-model' or args[0] == '-m'):
        args[1] = args[1].lower()
        model = None
        use_encoded_responses = False

        # if args[1] == 'snn':
        #     model = Siamese()
        #     use_encoded_responses = True
        # elif args[1] == 'lda':
        #     print('This model type has been removed')
        #     exit(0)
        if args[1] == 'gpt-3':
            model = GPT3('GPT3', None)
        # elif args[1] == 'gpt-3-etown':
        #     final_doc_list = pickle.load(open('data/EtownQAData.pkl', 'rb'))
        #     model = GPT3('ETOWN', final_doc_list)
        # elif args[1] == 'gpt-neo':
        #     model = GPTNeo('2.7B')
        # elif args[1] == 'gpt-neo-etown':
        #     print('This model type has been removed')
        #     exit(0)
        else:
            print('Please specify a valid model parameter.\nExample: [SNN, LDA, GPT-3, GPT-3-Etown, GPT-Neo, GPT-Neo-Etown]')
            exit(0)

        while True:
            listen(model, False, use_encoded_responses)
    else:
        print('Please specify valid start parameters.\nExample: python3 main.py -model [SNN, LDA, GPT-3, GPT-3-Etown, GPT-Neo, GPT-Neo-Etown]')
        exit(0)


# Press the green button to run the script
if __name__ == '__main__':
    main()
