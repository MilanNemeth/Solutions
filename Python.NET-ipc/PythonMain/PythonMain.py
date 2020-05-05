import socket
import json
import time
import random

UDP_IP = "127.0.0.2"
UDP_PORT = 50500
remote_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

t = time.clock()
t = time.clock() - t # t is wall seconds elapsed (floating point)
print(t)

for x in range(1000):
    try:
        tupy = (random.random(),random.random(),random.random())
        b_arr = bytearray(json.dumps(tupy).encode())
        remote_socket.sendto(b_arr, (UDP_IP, UDP_PORT))
        time.sleep(0.1)
        t = time.clock() - t
        print(t)
    except :
        print("Something went wrong!")
