#! /usr/bin/env python3
# -*- coding: utf-8 -*-

import ftrobopy
import paho.mqtt.client as mqtt
from time import sleep

class HighBayStorageRack:
    def __init__(self, host = "65000"):
        self.txt = ftrobopy.ftrobopy(host, 65000)      # connect to TXT's IO controller
        self._fork_offset = 180
        self._positions = [
            # Each position is characterized by an x and a y component
            # The first entry is the in and output,
            # then follow the 6 storage bays
            (0, 40),
            (3100, 1910), (5025, 1910),
            (3100, 950),  (5025, 950),
            (3100, -10),   (5025, -10)
        ]

        self._x_motor = self.txt.motor(1)
        self._z_motor = self.txt.motor(3)
        self._fork_motor = self.txt.motor(2)
        self._fork_front_button = self.txt.input(4)
        self._fork_back_button = self.txt.input(2)
        self._x_origin_button = self.txt.input(1)
        self._z_origin_button = self.txt.input(3)

        self._origin()

    
    def _position_x(self, x):
        self._x_motor.setSpeed( -512 if x > 0 else 512 )
        self._x_motor.setDistance(abs(x))
        while not self._x_motor.finished():
            sleep(0.01)
        self._x_motor.stop()
        

    def _position_z(self, z):
        self._z_motor.setSpeed( -512 if z > 0 else 512 )
        self._z_motor.setDistance(abs(z))
        while not self._z_motor.finished():
            sleep(0.01)

    def _position_xz(self, x, z):
        self._z_motor.setSpeed( -512 if z > 0 else 512 )
        self._x_motor.setSpeed( -512 if x > 0 else 512 )
        self._x_motor.setDistance(abs(x))
        self._z_motor.setDistance(abs(z))
        while True:
            x_finished = self._x_motor.finished()
            z_finished = self._z_motor.finished()
            if x_finished:
                self._x_motor.stop()
            if z_finished:
                self._z_motor.stop
            if x_finished and z_finished:
                return


    def _origin(self):
        self._fork_back()
        self._x_motor.setDistance(0)
        self._z_motor.setDistance(0)
        while True:
            x_zero = self._x_origin_button.state()
            z_zero = self._z_origin_button.state()
            if not x_zero:
                self._x_motor.setSpeed(512)
            else:
                self._x_motor.stop()
            if not z_zero:
                self._z_motor.setSpeed(512)
            else:
                self._z_motor.stop()
            if x_zero and z_zero:
                return

    def _fork_front(self):
        while not self._fork_front_button.state():
            self._fork_motor.setSpeed(-512)
        self._fork_motor.stop()
    def _fork_back(self):
        while not self._fork_back_button.state():
            self._fork_motor.setSpeed(512)
        self._fork_motor.stop()

    def _fork_up(self):
        self._position_z(self._fork_offset)
    def _fork_down(self):
        self._position_z(-self._fork_offset)

    def _pickup(self):
        self._fork_front()
        self._fork_up()
        self._fork_back()

    def _place(self):
        self._fork_front()
        self._fork_down()
        self._fork_back()
        self._origin()

    def _to_position(self, pos):
        target = self._positions[pos]
        self._position_xz(target[0], target[1])

    def store_in(self, pos):
        self._to_position(0)
        self._pickup()
        self._to_position(pos)
        self._place()


    def store_out(self, pos):
        self._to_position(pos)
        self._pickup()
        self._origin()
        self._to_position(0)
        self._fork_up()
        self._place()


def on_connect(client, userdate, flags, rc):
    if rc == 0:
        print("Connected to broker")
        client.subscribe("commands")
    else:
        print("Connection failed")


def on_message(client, userdate, message):
    payload = message.payload.decode('utf-8')

    print(f'Message received: {payload}')

    command = payload[:2]
    shelf_id = int(payload[2])

    if command == "si":
        hbsr.store_in(shelf_id)
    elif command == "so":
        hbsr.store_out(shelf_id)


def loop_mqtt():
    broker_address = "localhost"
    port = 1883
    user = "me"
    password = "absdef"

    client = mqtt.Client("Python")
    client.username_pw_set(user, password)
    client.on_connect = on_connect
    client.on_message = on_message
    client.connect(broker_address, port, 60)
    client.loop_forever()


if __name__ == "__main__":
    global hbsr
    hbsr = HighBayStorageRack("auto")
    loop_mqtt()
    