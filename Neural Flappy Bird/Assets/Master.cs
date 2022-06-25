using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Master : MonoBehaviour{
    
    public static Master inst;
    
    public int[] topology = new int[]{4, 4, 4, 2};
    public int population = 100;
    public float mutationRate = 0.01f;
    public int generation = 0;
    public Vector3 spawnPos = new Vector3(-5f, 3f, 0f);

    public float timer;
    public GameObject birdPrefab;

    public List<Puppet> puppets = new List<Puppet>();

    void Awake(){
        inst = this;
    }

    void Start(){
        CreatePopulation();
    }

    void Update(){
        timer += Time.deltaTime;
        if(timer > 100f || puppets.All(p => !p.bird.isAlive)){
            CullAndBreed();
            timer = 0f;
        }
    }

    void CreatePopulation(){
        for(int i = 0; i < population; i++){
            GameObject go = Instantiate(birdPrefab, spawnPos, Quaternion.identity);
            var puppet = go.GetComponent<Puppet>();
            puppet.brain = new NeuralNetwork(topology);
            puppets.Add(puppet);
        }
    }

    void CullAndBreed(){
        generation++;
        puppets = puppets.OrderByDescending(x => x.fitness).ToList();
        
        //print the mean fitness of all the population
        print("Generation: " + generation + " Mean Fitness: " + puppets.Average(x => x.fitness));
        
        List<Puppet> newPuppets = new List<Puppet>();
        var parentA = puppets[0];
        newPuppets.Add(parentA);
        for(int i = 0; i < 3*population/4; i++){
            var parentB = puppets[i+1];
            var childA = Instantiate(birdPrefab, spawnPos, Quaternion.identity);
            var childB = Instantiate(birdPrefab, spawnPos, Quaternion.identity);
            var childAPuppet = childA.GetComponent<Puppet>();
            var childBPuppet = childB.GetComponent<Puppet>();
            childAPuppet.brain = parentA.brain.Crossover(parentB.brain);
            childBPuppet.brain = parentB.brain.Crossover(parentA.brain);
            childAPuppet.brain.Mutate(mutationRate);
            childBPuppet.brain.Mutate(mutationRate);
            newPuppets.Add(childAPuppet);
            newPuppets.Add(childBPuppet);
        }

        puppets.RemoveAt(0);
        puppets.ForEach(x => Destroy(x.gameObject));
        puppets = newPuppets;
        puppets.ForEach(x => x.fitness = 0f);
        LevelManager.inst.ClearObstacles();
    }

    
}