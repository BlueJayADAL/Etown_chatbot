import pickle
import numpy as np
import pandas as pd
import utils.DataCleanser as dc

import tensorflow as tf
from tensorflow import keras
import keras.backend as K

from keras.preprocessing.text import Tokenizer
from keras.preprocessing.sequence import pad_sequences

from keras.layers import Input, Concatenate, Conv2D, Flatten, Dense, Embedding, LSTM
from keras.models import Model
from keras.regularizers import l2
from keras.models import Sequential
from keras.layers import Conv2D, ZeroPadding2D, Activation, Input, concatenate
from keras.models import Model

from keras.layers import BatchNormalization
from keras.layers.pooling import MaxPooling2D
from keras.layers.merge import Concatenate
from keras.layers.core import Lambda, Flatten, Dense
from keras.layers import Input, Dense, Flatten, GlobalMaxPool2D, GlobalAvgPool2D, Concatenate, Multiply, Dropout, Subtract, Add, Conv2D


class Siamese:
    model, tokenizer, questions = None, None, None

    def __init__(self):
        """
        Modified Siamese Network for computing similar sentence classification
        Original code snippets courtesy of Prabhnoor Singh
        https://github.com/prabhnoor0212
        ------------------------------------------------------------------------
        Model training occurs in 'Jupyter Notebooks/Siamese_Preparation.ipynb'
        """

        # Initialize class variables
        self.model = keras.models.load_model('data/snn.h5', custom_objects={"K": K})
        self.tokenizer = pickle.load(open('data/tokenizer.pkl', 'rb'))
        self.questions = pickle.load(open('data/question_categories.pkl', 'rb'))

    def evaluate(self, text):
        text = self.transform_text(text)

        # Account for misunderstood input
        if text is None:
            return -1

        return np.argmax(self.model.predict(text))

    """ Transforms an inputted text into model accepted format """
    def transform_text(self, text):
        d = {0: [text] * len(self.questions), 1: self.questions}
        eval_df = pd.DataFrame(data=d)
        eval_df[0] = eval_df[0].astype(str)
        eval_df[1] = eval_df[1].astype(str)

        eval_q1_seq = self.tokenizer.texts_to_sequences(eval_df[0].values)
        eval_q2_seq = self.tokenizer.texts_to_sequences(eval_df[1].values)

        # No valid vocabulary found
        if len(eval_q1_seq[0]) == 0:
            return None

        len_vec = [len(sent_vec) for sent_vec in eval_q2_seq]
        max_len = np.max(len_vec)

        eval_q1_seq = pad_sequences(eval_q1_seq, maxlen=max_len, padding='post')
        eval_q2_seq = pad_sequences(eval_q2_seq, maxlen=max_len, padding='post')

        return eval_q1_seq, eval_q2_seq
