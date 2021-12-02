// TC2008B Modelación de Sistemas Multiagentes con gráficas computacionales
// C# client to interact with Python server via POST
// Sergio Ruiz-Loza, Ph.D. March 2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class WebClient : MonoBehaviour
{   
    //Semaforos
    List<String> semaforos = new List<string>();

    private string[] strs;
    List<List<Vector3>> positions;

    public GameObject[] spheres;
    public GameObject semaforo1, semaforo2, semaforo3, semaforo4;
    public float timeToUpdate = 5.0f;
    private float timer;
    public float dt;
    private int estadoRepeticion = 0; 
    private bool paso = false;

    // IEnumerator - yield return
    IEnumerator SendData(string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585/multiagentes";
        //using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("Content-Type", "text/html");
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();          // Talk to Python
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);    // Answer from Python
                //Debug.Log("Form upload complete!");
                //Data tPos = JsonUtility.FromJson<Data>(www.downloadHandler.text.Replace('\'', '\"'));
                //Debug.Log(tPos);
                List<Vector3> newPositions = new List<Vector3>();
                string txt = www.downloadHandler.text.Replace('\'', '\"');
                txt = txt.TrimStart('"', '{', 'd', 'a', 't', 'a', ':', '[');
                txt = "{\"" + txt;
                txt = txt.TrimEnd(']', '}');

                //Comentar la línea 54 si es solo un agente
                //txt = txt + '}';
                if(spheres.Length != 1){
                    txt = txt + '}';
                }

                string[] strs = txt.Split(new string[] { "}, {" }, StringSplitOptions.None);
                Debug.Log("strs.Length:" + strs.Length);
                for (int i = 0; i < strs.Length; i++)
                {
                    strs[i] = strs[i].Trim();
                    if (i == 0) strs[i] = strs[i] + '}';
                    else if (i == strs.Length - 1) strs[i] = '{' + strs[i];
                    else strs[i] = '{' + strs[i] + '}';
                    Vector3 test = JsonUtility.FromJson<Vector3>(strs[i]);
                    newPositions.Add(test);
                }

                List<Vector3> poss = new List<Vector3>();
                for (int s = 0; s < spheres.Length; s++)
                {
                    //spheres[s].transform.localPosition = newPositions[s];
                    poss.Add(newPositions[s]);
                    //paso = false;
                }
                positions.Add(poss);
                paso = false;
            }
        }
        paso = false;

    }

     //Recibir datos de semaforos
    IEnumerator SendDataSemaforo(string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585/semaforos";
        //using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            //www.SetRequestHeader("Content-Type", "text/html");
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest(); // Talk to Python
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);    // Answer from Python
                //Debug.Log("Form upload complete!");
                //Data tPos = JsonUtility.FromJson<Data>(www.downloadHandler.text.Replace('\'', '\"'));
                //Debug.Log(tPos);
                String estados;
                string txt = www.downloadHandler.text.Replace('\'', '\"');
                txt = txt.TrimStart('"', '{', 'd', 'a', 't', 'a', 'S', 'e', 'm', ':', '[');
                txt = "\"" + txt;
                txt = txt.TrimEnd(']', '}');

                strs = txt.Split(new string[] {"[, ]"}, StringSplitOptions.None);
                

            }

        }
    }


    void EnciendeSemaforo(List<String> semaforos)
            {
                if (semaforos.Count > 1)
                {
                    string sem1 = semaforos[0];
                    string sem2 = semaforos[1];
                    string sem3 = semaforos[2];
                    string sem4 = semaforos[3];
                    Debug.Log(sem2);

                    //Semaforo 1
                    if (sem1 == "g")
                    {
                        //semaforo1.transform.GetChild(0).GetComponent<Material>().color = Color.green;
                        semaforo1.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            new Color(49/255f, 127/255f, 67/255f);
                        
                        semaforo1.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo1.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;

                    }
                    else if (sem1 == "y")

                    {
                        semaforo1.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo1.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            new Color(1f, 1f, 0/255f);
                        
                        semaforo1.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                    }
                    else
                    {
                        semaforo1.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo1.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;

                        semaforo1.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            new Color(1f, 0/255f, 0/255f);

                    }

                    // Semaforo 2
                    if (sem2 == "g")
                    {
                        semaforo2.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            new Color(49/255f, 127/255f, 67/255f);
                        
                        semaforo2.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo2.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                    }
                    else if (sem2 == "y")

                    {
                        semaforo2.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo2.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            new Color(1f, 1f, 0/255f);
                        
                        semaforo2.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;

                    }
                    else
                    {
                        semaforo2.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo2.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;

                        semaforo2.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            new Color(1f, 0/255f, 0/255f);

                    }

                    //Semaforo 3
                    if (sem3 == "g")
                    {
                        semaforo3.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            new Color(49/255f, 127/255f, 67/255f);
                        
                        semaforo3.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo3.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                    }
                    else if (sem3 == "y")

                    {
                        semaforo3.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo3.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            new Color(1f, 1f, 0/255f);
                        
                        semaforo3.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                    }
                    else
                    {
                        semaforo3.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo3.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;

                        semaforo3.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            new Color(1f, 0/255f, 0/255f);

                    }

                    //Semaforo 4
                    if (sem4 == "g")
                    {
                        semaforo4.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            new Color(49/255f, 127/255f, 67/255f);
                        
                        semaforo4.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo4.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                    }
                    else if (sem4 == "y")

                    {
                        semaforo4.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo4.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            new Color(1f, 1f, 0/255f);
                        
                        semaforo4.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;

                    }
                    else
                    {
                        semaforo4.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;
                        
                        semaforo4.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color =
                            Color.black;

                        semaforo4.transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color =
                            new Color(1f, 0/255f, 0/255f);
                    }
                }
            }


    IEnumerator ResetModel(string data, GameObject[] spheres)
    {
        Debug.Log("Entró a resetModel"); 
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585/resetModel";
        //using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("Content-Type", "text/html");
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();          // Talk to Python
            if (www.isNetworkError || www.isHttpError){
                Debug.Log(www.error);
            } 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        positions = new List<List<Vector3>>();
        Debug.Log(spheres.Length);
#if UNITY_EDITOR
        //string call = "WAAAAASSSSSAAAAAAAAAAP?";
        Vector3 fakePos = new Vector3(3.44f, 0, -15.707f);
        string json = EditorJsonUtility.ToJson(fakePos);
        //StartCoroutine(SendData(call));
        List<String> fakeEdo = new List<String> {"g", "r", "g", "r"};
        string jsonSem = EditorJsonUtility.ToJson(fakeEdo);
        StartCoroutine(SendData(json));
        StartCoroutine(SendDataSemaforo(jsonSem));
        timer = timeToUpdate;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        /*if(estadoRepeticion == 1){
            ActivateObjects(spheres); 
        }*/
        /*
         *    5 -------- 100
         *    timer ----  ?
         */
        timer -= Time.deltaTime;
        dt = 1.0f - (timer / timeToUpdate);

        if(timer < 0)
        {
#if UNITY_EDITOR
            timer = timeToUpdate; // reset the timer
            Vector3 fakePos = new Vector3(3.44f, 0, -15.707f);
            string json = EditorJsonUtility.ToJson(fakePos);
            List<String> fakeEdo = new List<String> {"g", "r", "g", "r"};
            string jsonSem = EditorJsonUtility.ToJson(fakeEdo);
            StartCoroutine(SendData(json));
            StartCoroutine(SendDataSemaforo(jsonSem));
#endif
        }


        if (positions.Count > 1)
        {
            semaforos = new List<string>();
            for (int s = 0; s < spheres.Length; s++)
            {
                // Get the last position for s
                List<Vector3> last = positions[positions.Count - 1];
                // Get the previous to last position for s
                List<Vector3> prevLast = positions[positions.Count - 2];
                // Interpolate using dt
                Vector3 interpolated = Vector3.Lerp(prevLast[s], last[s], dt);
                spheres[s].transform.localPosition = interpolated - new Vector3(0.0f,0.0f,0.0f);

                Vector3 dir = last[s] - prevLast[s];
                Vector3 vacio = new Vector3(0, 0, 0); 

                /*if(dir == vacio){
                    spheres[s].active = false;
                }*/ 

                spheres[s].transform.rotation = Quaternion.LookRotation(dir);
            }
            for (int i = 0; i < strs[0].Length; i++)
            {
                //Debug.Log(strs[0][i]);
                if (strs[0][i].ToString() == "g" || strs[0][i].ToString() == "r" || strs[0][i].ToString() == "y")
                {
                    semaforos.Add(strs[0][i].ToString());
                }
            
            }
                //string hola = semaforos[0];
                //Debug.Log("Strings " + hola);
                EnciendeSemaforo(semaforos);
        }
        //bool state = false; 
        int desactivados = 0; 
        for(int s = 0; s < spheres.Length; s++){
            if(spheres[s].active == false){
                //state = true; 
                desactivados++; 
                Debug.Log("Objeto desactivado"); 
            }
        }
        Debug.Log("Número de objetos: ");
        Debug.Log(spheres.Length); 
        Debug.Log("Desactivados: ");
        Debug.Log(desactivados);
        if(paso){
            for(int s = 0; s < spheres.Length; s++){
             spheres[s].SetActive(true);
            }
            paso = true;
        }

        if(spheres.Length == desactivados){
            desactivados = 0;
            StopCoroutine("SendData");
            Debug.Log("Todos los objetos están desactivados"); 
            Vector3 fakePos = new Vector3(3.44f, 0, -15.707f);
            string json = EditorJsonUtility.ToJson(fakePos);
            StartCoroutine(ResetModel(json, spheres));
            StopCoroutine("ResetModel");
            StartCoroutine(SendData(json));
            paso = true;
            //ResetModel(json);
        }
    }
}
