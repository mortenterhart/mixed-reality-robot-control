import sys
from flask import Flask, abort
from hbsr import HighBayStorageRack

app = Flask(__name__)
try:
    hbsr = HighBayStorageRack(host="192.168.8.2")
except Exception as e:
    sys.exit("Can't connect to TXT Controller: " + ( e.message if hasattr(e, "message") else str(e) ) + ". Aborting.")


@app.route("/hbsr")
def index():
    return """
    Usage:\n
    Make a GET request to \n
     \t   /hbsr/in/<int: id>\n
     \t   /hbsr/out/<int: id>\n
    to store in or out of the HighBayStorageRack\n
    """

@app.route("/hbsr/in/<int:position>")
def store_in(position):
    if not (position > 0 and position <= 6):
        abort(404)
    hbsr.store_in(position)
    return "OK"

@app.route("/hbsr/out/<int:position>")
def store_out(position):
    if not (position > 0 and position <= 6):
        abort(404)
    hbsr.store_out(position)
    return "OK"
