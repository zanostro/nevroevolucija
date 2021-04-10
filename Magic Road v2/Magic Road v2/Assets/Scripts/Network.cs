using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[Serializable]
public class Network : ICloneable
{
    public bool active = false;

    private double defaultMutationFactor = 0.02;

    public double[][] output;
    private double[][][] weights;        //weights[layer][neuron][prevNeuron]

    public double[][] bias;

    private double[][] error_signal;
    private double[][] output_derivative;

    //statične spremelnljivke
    private int[] NETWORK_LAYER_SIZES;
    private int INPUT_SIZE;
    private int OUTPUT_SIZE;
    private int NETWORK_SIZE;

    
    public Network(int[] NETWORK_LAYER_SIZES)
    {
        this.NETWORK_LAYER_SIZES = NETWORK_LAYER_SIZES;
        this.INPUT_SIZE = NETWORK_LAYER_SIZES[0];
        this.OUTPUT_SIZE = NETWORK_LAYER_SIZES[NETWORK_LAYER_SIZES.Length - 1];
        this.NETWORK_SIZE = NETWORK_LAYER_SIZES.Length;
        this.bias = new double[NETWORK_SIZE][];

        this.output = new double[NETWORK_SIZE][];
        this.weights = new double[NETWORK_SIZE][][];
      

        this.error_signal = new double[NETWORK_SIZE][];
        this.output_derivative = new double[NETWORK_SIZE][];

        for (int i = 0; i < NETWORK_SIZE; i++)
        {
            this.output[i] = new double[NETWORK_LAYER_SIZES[i]];
            this.error_signal[i] = new double[NETWORK_LAYER_SIZES[i]];
            this.output_derivative[i] = new double[NETWORK_LAYER_SIZES[i]];

            this.bias[i] = NetworkTools.createRandomArray(NETWORK_LAYER_SIZES[i], -1, 1);

            if (i > 0)
            {
                weights[i] = NetworkTools.createRandomArray(NETWORK_LAYER_SIZES[i], NETWORK_LAYER_SIZES[i - 1], - 1, 1);
            }  // prvi layer nima weigths
        }
        active = true;
    }
    public double[] calculate(double[] input)
    {
        if (input.Length != this.INPUT_SIZE) return null;
        this.output[0] = input;
        for (int layer = 1; layer < NETWORK_SIZE; layer++)
        {
            for (int neuron = 0; neuron < NETWORK_LAYER_SIZES[layer]; neuron++)
            {
                double sum = bias[layer][neuron];
                
                for (int prevNeuron = 0; prevNeuron < NETWORK_LAYER_SIZES[layer - 1]; prevNeuron++)
                {
                    sum += output[layer - 1][prevNeuron] * weights[layer][neuron][prevNeuron];
                }
                output[layer][neuron] = sigmoid(sum);
                output_derivative[layer][neuron] = output[layer][neuron] * (1 - output[layer][neuron]);
            }
        }
        return output[NETWORK_SIZE - 1];
    }



    //naredi deep copy NN
    public object Clone()
    {
        using (MemoryStream stream = new MemoryStream())
        {
            if (this.GetType().IsSerializable)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
            return null;
        }
    }


    public void mutateNetwork() 
    {
        mutateNetwork(defaultMutationFactor);
    }


    public void mutateNetwork(double mutationFactor)
    {
        if (mutationFactor > 1) mutationFactor = 1;
        if (mutationFactor < 0) mutationFactor = 0;

        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
        for (int layer = 1; layer < NETWORK_SIZE; layer++)
        {
            for (int neuron = 0; neuron < NETWORK_LAYER_SIZES[layer]; neuron++)
            {
                for (int prevNeuron = 0; prevNeuron < NETWORK_LAYER_SIZES[layer - 1]; prevNeuron++)
                {
                    double x = rnd.NextDouble();
                    if (mutationFactor >= x)
                    {
                        weights[layer][neuron][prevNeuron] = (rnd.NextDouble() * 2) - 1;
                    }
                }
            }
        }
    }

    public void crossBreed(Network parent2)
    {
        //cross za w
        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
        for (int layer = 1; layer < NETWORK_SIZE; layer++)
        {
            for (int neuron = 0; neuron < NETWORK_LAYER_SIZES[layer]; neuron++)
            {
                for (int prevNeuron = 0; prevNeuron < NETWORK_LAYER_SIZES[layer - 1]; prevNeuron++)
                {
                    double x = rnd.NextDouble();
                    if (x >= 0.5)                       //50% za vsakega starsa
                    {
                        weights[layer][neuron][prevNeuron] = parent2.weights[layer][neuron][prevNeuron];
                    }
                }
            }
        }
        
        //cross za b
       for(int i = 0; i< NETWORK_SIZE; i++)            
        {
            for (int l = 0; l < bias[i].Length - 1; l++)
            {
                double x = rnd.NextDouble();
                x = rnd.NextDouble();
                if (x >= 0.5)                           //50% za vsakega starsa
                {
                    bias[i][l] = parent2.bias[i][l];
                }
            }   
       }
    }

    private double sigmoid(double x) { return 1d / (1 + Math.Exp(-x)); }

}
