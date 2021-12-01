from flask import Flask, render_template, request, jsonify
import json, logging, os, atexit

from ModeloTrafico import TraficModel

ancho =40
alto = 40
N = 4
app = Flask(__name__, static_url_path='')
model = TraficModel(N,ancho,alto)
def positionsToJSON(ps):
    posDICT = []
    for p in ps:
        pos = {
            "x" : p[0],
            "z" : p[1],
            "y" : p[2]
        }
        posDICT.append(pos)
    return json.dumps(posDICT)

# On IBM Cloud Cloud Foundry, get the port number from the environment variable PORT
# When running this app on the local machine, default the port to 8000
port = int(os.getenv('PORT', 8585))

@app.route('/')
def root():
    return jsonify([{"message":"Hello World from IBM Cloud!"}])
    
@app.route('/multiagentes')
def multiagentes():
    '''if(model.schedule.steps > 10):
        model.__init__(N, ancho, alto)'''
    positions = model.step()
    print(model.schedule.steps)
    respuesta = "{\"data\":" + positionsToJSON(positions) + "}"
    return respuesta

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=port, debug=True)