import numpy as np
import tensorflow as tf
from tensorflow.python.keras import Input
from tensorflow.python.keras.layers import Conv2D, BatchNormalization, MaxPooling2D, Dropout, Flatten, Dense
from tensorflow.keras.models import Model


class MLNLP:
    model = None

    def __init__(self):
        pass


    def train(self, X_train, y_train, X_val, y_val, n_batch, n_epochs, learning_rate, decay_rate, save_dir):
        pass

    def classify(self, data):
        pass
