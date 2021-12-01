# -*- coding: utf-8 -*-
"""
Equipo: 
Liam Garay Monroy A01750632
Jorge Chávez Badillo A01749448
Amy Murakami Tsutsumi A01750185
Ariadna Jocelyn Guzmán Jiménez A01749373
"""

from mesa import Agent, Model, model
from mesa.time import RandomActivation
from mesa.space import Grid, SingleGrid
import random
from timeit import default_timer as timer

class CarAgent(Agent):
    """Modelo para un automóvil"""
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)
        random.seed()
        self.direccion = -1 # Frente 0, Derecha 1, Izquierda 2, Atras 3
        self.sentido = -1 # Derecha 0, Izquierda 1, Arriba 2, Abajo 3
        # El sentido toma en consideracion que las columnas crecen a la derecha y los renglones
        # crecen hacia arriba
        self.ancho = 0
        self.posicionInicial = ()

    def isFreeMyDirection(self, listFreeSpaces, list_possible_steps):
        """Función que se encarga de buscar si existen espacios libres"""
        if(self.sentido == 0): # Derecha
            if(self.direccion == 0): # Frente
                return listFreeSpaces[7], list_possible_steps[7]
            elif (self.direccion == 1): # Derecha
                return listFreeSpaces[6],list_possible_steps[6]
            elif(self.direccion == 2): # Izquierda
                return listFreeSpaces[8], list_possible_steps[8]
            elif(self.direccion == 3): # Atras
                return listFreeSpaces[1], list_possible_steps[1]
            else:
                print("Error en self.direccion")
                return False, (-1,-1)
        elif(self.sentido == 1): # Izquierda
            if(self.direccion == 0): # Frente
                return listFreeSpaces[1], list_possible_steps[1]
            elif(self.direccion == 1): # Derecha
                return listFreeSpaces[2], list_possible_steps[2]
            elif(self.direccion == 2): # Izquierda
                return listFreeSpaces[0], list_possible_steps[0]
            elif(self.direccion == 3): # Atras
                return listFreeSpaces[7], list_possible_steps[7]
            else:
                print("Error en self.direccion")
                return False, (-1,-1)
        elif(self.sentido == 2): # Arriba
            if(self.direccion == 0): # Frente
                return listFreeSpaces[5], list_possible_steps[5]
            elif (self.direccion == 1): # Derecha
                return listFreeSpaces[8], list_possible_steps[8]
            elif(self.direccion == 2): # Izquierda
                return listFreeSpaces[2], list_possible_steps[2]
            elif(self.direccion == 3): # Atras
                return listFreeSpaces[3], list_possible_steps[3]
            else:
                print("Error en self.direccion")
                return False,(-1,-1)
        elif(self.sentido == 3): # Abajo
            if(self.direccion == 0): # Frente
                return listFreeSpaces[3], list_possible_steps[3]
            elif (self.direccion == 1): # Derecha
                return listFreeSpaces[0], list_possible_steps[0]
            elif(self.direccion == 2): # Izquierda
                return listFreeSpaces[6], list_possible_steps[6]
            elif(self.direccion == 3): # Atras
                return listFreeSpaces[5], list_possible_steps[5]
            else:
                print("Error en self.direccion")
                return False, (-1,-1)
        else:
            print("Error en self.sentido")
            return False, (-1,-1)
    
    def preguntaSemaforo(self, semaforos):
        '''
        Número Semáforo     Posición Lista Semáforos    Coordenadas    
        Semáforo 1          0                           (3, 3)
        Semáforo 2          1                           (6, 3)
        Semáforo 3          2                           (3, 6)
        Semáforo 4          3                           (6, 6)                   
        '''
        # Horizontal 
        # Semaforo 1
        if((self.pos[0] >= 1 and self.pos[0] < (self.ancho//2+1)) and self.pos[1] == (self.ancho//2-1)):
                if(semaforos[1].estado == "g" or semaforos[1].estado == "y"):
                    self.move()
        # Semaforo 3
        if((self.pos[0] <= (self.ancho - 2) and self.pos[0] >= (self.ancho//2 - 1)) and self.pos[1] == (self.ancho//2)):
                if(semaforos[2].estado == "g" or semaforos[2].estado == "y"):
                    self.move()

        #Vertical 
        # Semaforo 2
        if(self.pos[0] == self.ancho//2 and (self.pos[1] >= 1 and self.pos[1] <= self.ancho//2)):
                if(semaforos[3].estado == "g" or semaforos[3].estado == "y"):
                    self.move()
        # Semaforo 0
        if(self.pos[0] == self.ancho//2 - 1 and (self.pos[1] >= self.ancho//2 - 1 and self.pos[1] <= self.ancho - 2)):
                if(semaforos[0].estado == "g" or semaforos[0].estado == "y"):
                    self.move()

    def revisaUbicacion(self, x, y, ancho):
        self.posicionInicial = (x, y)
        # Calle Horizontal de ida desde cuadrante inferior izquierdo, Fijo el eje y
        if((self.posicionInicial[0] >= 1 and self.posicionInicial[0] <= (ancho//2) - 2) and (self.posicionInicial[1] == ancho//2 - 1)): 
            return (0, 0)
        # Calle Horizontal de vuelta desde cuadrante superior derecho, Fijo el eje y
        if((self.posicionInicial[0] >= (ancho//2) + 1 and self.posicionInicial[0] <= (ancho - 2)) and (self.posicionInicial[1] == ancho//2)): 
            return (0, 1)
        # Calle Vertical de ida desde cuadrante inferior derecho, Fijo el eje x
        if((self.posicionInicial[0] == ancho//2) and (self.posicionInicial[1] >= 1 and self.posicionInicial[1] <= ancho -2)): 
            return (0, 2)
        # Calle Vertical de vuelta desde cuadrante superior izquierdo, Fijo el eje x
        if((self.posicionInicial[0] == ancho//2 - 1) and (self.posicionInicial[1] >= ancho//2 - 2 and self.posicionInicial[1] <= ancho -2)): 
            return (0, 3)
        else:
            return (-100, -100)
    
    def move(self):
        possible_steps = self.model.grid.get_neighborhood(
            self.pos,
            moore = True, # 8 Conectado. Orden: Arriba Izquierda, Centro, Derecha; Enmedio Izquierda y Derecha; Abajo Izquierda, Centro, Derecha
            include_center = True) # Incluye su posición
        print(possible_steps)
        myPosition = possible_steps[4]

        # Verificar cuales espacios a mi alrededor se encuntran ocupados 
        freeSpaces = []
        for pos in possible_steps:
            freeSpaces.append(self.model.grid.is_cell_empty(pos))
        print(freeSpaces)

        # Movimiento tomando en consideración la dirección y si está libre ese espacio
        free, newPos = self.isFreeMyDirection(freeSpaces, possible_steps)
        if free:
            self.model.grid.move_agent(self, newPos)
            print(f"Se mueve de {myPosition} a {newPos} porque va hacia {self.direccion}\n")
        else:
            print(f"No se puede mover en sentido {self.sentido}, ubicación ocupada.")
            nuevoSentido = random.randint(0, 3)
            while (nuevoSentido == self.sentido):
                nuevoSentido = random.randint(0, 3)
            #self.sentido = nuevoSentido
            #self.sentido = 1
            print(f"Cambiando sentido a {self.sentido}\n")
    
    def step(self):
        """ En cada paso moverse aleatoriamente """
        # self.direccion = random.randint(0, 3)
        #self.direccion = 0
        # Horizontal 
        # Semáforo 1 
        if((self.pos[0] >= self.ancho//2+1) and (self.pos[1] == self.ancho//2-1)):
           self.move()
        # Semáforo 3
        if((self.pos[0] <= self.ancho//2 - 2) and (self.pos[1] == self.ancho//2)):
            self.move()

        # Vertical 
        # Semáforo 2
        if((self.pos[0] == self.ancho//2) and (self.pos[1] >= self.ancho//2 + 1)):
            self.move()
        # Semáforo 0
        if((self.pos[0] == self.ancho//2 - 1) and (self.pos[1] <= self.ancho//2 - 2)):
            self.move()
        print(f"Agente: {self.unique_id} movimiento {self.direccion}")
        #self.move()

class Semaforo(Agent):
    """Modelo para los semáforos"""
    def __init__(self,unique_id,model):
        super().__init__(unique_id,model)
        self.estados=["g", "y", "r"]
        self.estado = ""

    def getEstado(self):
        return self.estado
    
    def step(self):
        pass

    def cambioVerde(self):
        self.estado = self.estados[0]

    def cambioAmarillo(self):
        self.estado = self.estados[1]

    def cambioRojo(self):
        self.estado = self.estados[2]
    

class ObstacleAgent(Agent):
    """ Modelo para un Obstaculo """
    def __init__(self, unique_id, model):
        super().__init__(unique_id, model)

    def step(self):
        # The agent's step will go here.
        pass

class TraficModel(Model):
    """Modelo para los automóviles"""
    def __init__(self, N, ancho, alto):
        self.num_agents = N
        self.grid = SingleGrid(ancho, alto, False) # NO Es Toroidal
        self.schedule = RandomActivation(self)
        self.running = True # Para la visualizacion
        self.start = timer()
        self.agentesSemaforos = []
        self.carros = []
        self.ancho = ancho

        # Crear obstaculos en los limites del grid
        numObs = (ancho * 2) + (alto * 2 - 4)
        print("Número de obstáculos: ", numObs)
        listaPosLimite = []
        semaforos = []

        # Las dos columnas límite
        for col in [0, ancho-1]:
            for ren in range(alto):
                listaPosLimite.append((col, ren))
        print(listaPosLimite)

        # Los dos renglones limite
        for col in range(1, ancho-1):
            for ren in [0, alto-1]:
                listaPosLimite.append((col, ren))

        # Intersección cuadrante inferior izquierdo 
        for ren in range(1, (ancho//2 - 1)):
            for col in range(1, (ancho//2 - 1)):
                if(ren == (ancho//2 - 2) and col == (ancho//2 - 2)):
                    semaforos.append((ren, col))
                else:
                    listaPosLimite.append((ren, col))

        # Intersección cuadrante inferior derecho 
        for ren in range((ancho//2 + 1), ancho - 1):
            for col in range(1, (ancho//2 - 1)):
                if(ren == (ancho//2 + 1) and col == ancho//2 - 2):
                    semaforos.append((ren, col))
                else:
                    listaPosLimite.append((ren, col))

        # Intersección cuadrante superior izquierdo 
        for ren in range(1, (ancho//2 - 1)):
            for col in range((ancho//2 + 1), ancho - 1):
                if(ren == (ancho//2 - 2) and col == ancho//2 + 1):
                    semaforos.append((ren, col))
                else:
                    listaPosLimite.append((ren, col))
        
        # Intersección cuadrante superior derecho 
        for ren in range((ancho//2 + 1), ancho - 1):
            for col in range((ancho//2 + 1), ancho - 1):
                if(ren == (ancho//2 + 1) and col == ancho//2 + 1):
                    semaforos.append((ren, col))
                else:
                    listaPosLimite.append((ren, col))

        # Crear los obstáculos 
        #for i in range(numObs):
        print("----------------ListaPosLimite----------------------")
        print("Número de obstáculos:", len(listaPosLimite))
        for i in range(len(listaPosLimite)):
            a = ObstacleAgent(i, self)
            self.schedule.add(a)
            self.grid.place_agent(a, listaPosLimite[i])
            print(listaPosLimite[i])

        # Crear los semáforos 
        print("----------------Semáforos----------------------")
        print("Número de semáforos:", len(semaforos))
        for i in range(len(semaforos)):
            s = Semaforo(i + 2000, self) # La numeracion de los semáforos empieza en el 2000
            self.agentesSemaforos.append(s)
            self.schedule.add(s)
            self.grid.place_agent(s, semaforos[i])
            print(semaforos[i])

        # Inicializar los semáforos en rojo y verde 
        self.agentesSemaforos[0].cambioVerde() 
        self.agentesSemaforos[3].cambioVerde()

        self.agentesSemaforos[2].cambioRojo()
        self.agentesSemaforos[1].cambioRojo() 

        # Create car agents
        print("----------------Automóviles----------------------")
        print("Número de automóviles:", self.num_agents)
        posicionesCarros = [(20, 1), (19, 38), (1, 19), (38, 20)]
        posicionesCarros10 = [(1, 4), (8, 5), (5, 1), (4, 8)]
        for i in range(self.num_agents):
            a = CarAgent(i + 10000, self) # La numeracion de los agentes empieza en el 1000
            self.carros.append(a)
            self.schedule.add(a)
            # Add the agent to a random empty grid cell
            x = self.random.randrange(self.grid.width)
            y = self.random.randrange(self.grid.height)
            while (not self.grid.is_cell_empty((x, y))):
                x = self.random.randrange(self.grid.width)
                y = self.random.randrange(self.grid.height)

            # Horizontal 
            #x1, y1 = 1, 4 # Ida 
            #x1, y1 = 8, 5 # Regreso

            # Vertical 
            #x1, y1 = 5, 1 # Ida 
            #x1, y1 = 4, 8 # Regreso

            #x1, y1 = 10, 1 20 20 
            #x1, y1 = 15, 1

            #x1, y1 = 20, 1
            x1, y1 = posicionesCarros[i][0], posicionesCarros[i][1] 
            self.grid.place_agent(a, (x1, y1)) # Ida
            
            # Revisar en que coordenada aparecen los agentes para asignar dirección y sentido
            direccion, sentido = a.revisaUbicacion(x1, y1, ancho)[0], a.revisaUbicacion(x1, y1, ancho)[1]
            a.direccion, a.sentido = direccion, sentido
            a.ancho = ancho
            print(a.direccion, a.sentido)

    def step(self):
        """Advance the model by one step."""
        self.schedule.step()

        ps = []
        for carro in self.carros:
            carro.preguntaSemaforo(self.agentesSemaforos)
            xy = carro.pos
            p = [xy[0], xy[1], 0]  #XZY
            ps.append(p)
            print(carro.pos)

        end = timer()
        elapsed = (int)(end - self.start)
        print("Elapsed: ", elapsed)

        if(len(str(elapsed)) == 1):
            if(str(elapsed)[-1] == "7"):
                self.agentesSemaforos[0].cambioAmarillo() 
                self.agentesSemaforos[3].cambioAmarillo()
        else:
            selectSemaforo = str(elapsed)[-2]
            selectSemaforo = int(selectSemaforo)
            if(selectSemaforo%2 == 0):
                if(str(elapsed)[-1] == "0"):
                    self.agentesSemaforos[0].cambioVerde() 
                    self.agentesSemaforos[3].cambioVerde()

                    self.agentesSemaforos[2].cambioRojo()
                    self.agentesSemaforos[1].cambioRojo()
                elif(str(elapsed)[-1] == "7"):
                    self.agentesSemaforos[0].cambioAmarillo() 
                    self.agentesSemaforos[3].cambioAmarillo()
        
            else:
                if(str(elapsed)[-1] == "0"):
                    self.agentesSemaforos[2].cambioVerde() 
                    self.agentesSemaforos[1].cambioVerde()

                    self.agentesSemaforos[0].cambioRojo()
                    self.agentesSemaforos[3].cambioRojo()
                elif(str(elapsed)[-1] == "7"):
                    self.agentesSemaforos[2].cambioAmarillo() 
                    self.agentesSemaforos[1].cambioAmarillo()
        return ps