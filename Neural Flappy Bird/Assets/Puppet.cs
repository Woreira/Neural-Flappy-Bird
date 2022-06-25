using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet : MonoBehaviour{
    public NeuralNetwork brain;
    public float fitness = 0f;
    public BirdController bird;

    void Awake(){
        bird = GetComponent<BirdController>();
        brain = new NeuralNetwork(Master.inst.topology);
    }

    void Update(){
        if(!bird.isAlive) return;
        
        float[] inputs = new float[4];
        inputs[0] = bird.transform.position.y;
        inputs[1] = bird.rb.velocity.y;
        inputs[2] = LevelManager.nextObstacle.transform.position.x;
        inputs[3] = LevelManager.nextObstacle.transform.position.y;
        float[] outputs = brain.FeedForward(inputs);
        if(outputs[0] > outputs[1]){
            bird.Jump();
        }

        fitness += Time.deltaTime;
        fitness -= Vector3.Distance(bird.transform.position, LevelManager.nextObstacle.transform.position);
    }

}