using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork{
    public Neuron[][] neurons;
    public float[][] weights;
    public int[] topology;

    public NeuralNetwork(int[] topology){
        this.topology = topology;
        neurons = new Neuron[topology.Length - 1][];
        for(int i = 0; i < neurons.Length; i++){
            neurons[i] = new Neuron[topology[i]];
            for(int j = 0; j < neurons[i].Length; j++){
                neurons[i][j] = new Neuron();
            }
        }

        weights = new float[neurons.Length - 1][];
        for(int i = 0; i < weights.Length; i++){
            weights[i] = new float[topology[i]*topology[i+1]];
            for(int j = 0; j < weights[i].Length; j++){
                weights[i][j] = Random.Range(-0.5f, 0.5f);
            }
        }
    }

    public float[] FeedForward(float[] inputs){
        for(int i = 0; i < inputs.Length; i++){
            neurons[0][i].output = inputs[i];
        }

        for(int i = 1; i < neurons.Length; i++){
            for(int j = 0; j < neurons[i].Length; j++){
                float sum = 0;
                for(int k = 0; k < neurons[i-1].Length; k++){
                    sum += neurons[i-1][k].output * weights[i-1][k*neurons[i].Length + j];
                }
                neurons[i][j].output = Sigmoid(sum);
            }
        }

        float[] outputs = new float[neurons[neurons.Length-1].Length];
        for(int i = 0; i < outputs.Length; i++){
            outputs[i] = neurons[neurons.Length-1][i].output;
        }
        return outputs;
    }

    public void Mutate(float mutationRate){
        for(int i = 0; i < weights.Length; i++){
            for(int j = 0; j < weights[i].Length; j++){
                if(Random.Range(0.0f, 1.0f) < mutationRate){
                    weights[i][j] = Random.Range(-0.5f, 0.5f);
                }
            }
        }
    }

    //pick weights at random from each of the networks
    public NeuralNetwork Crossover(NeuralNetwork partner){
        NeuralNetwork child = new NeuralNetwork(partner.topology);
       
        for(int i = 0; i < child.weights.Length; i++){
            child.weights[i] = new float[weights[i].Length];
            for(int j = 0; j < child.weights[i].Length; j++){
                if(Random.Range(0.0f, 1.0f) < 0.5f){
                    child.weights[i][j] = weights[i][j];
                }else{
                    child.weights[i][j] = partner.weights[i][j];
                }
            }
        }

        return child;
    }

    public float Sigmoid(float x){
        return 1.0f / (1.0f + Mathf.Exp(-x));
    }
}

public class Neuron{
    public float output;
    public float bias;

    public Neuron(){
        bias = Random.Range(-0.5f, 0.5f);
    }
}