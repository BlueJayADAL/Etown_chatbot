import time

# from models.GPT3 import GPT3
from server.ServerSocket import ServerSocket
# from utils import DataCleanser as dc


def ServerApp():
    while True:
        try:
            ss = ServerSocket(4824)
            ss.listen()
        except Exception as e:
            print('\nWaiting 30 seconds for port to open...\n')
            time.sleep(30)


# Press the green button to run the script
if __name__ == '__main__':
    ServerApp()
