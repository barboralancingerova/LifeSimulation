using System.Reflection.Metadata.Ecma335;

public class NeuralNetwork
{
    public double[] Weights {get; } 
    public static double[] Forward(double[] inputs, double[] weights, int hiddenSize, int outputSize)
    {
        double[] hidden = new double[hiddenSize];
        for (int j = 0; j < hiddenSize; j++)
        {
            double sum = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += weights[j * inputs.Length + i] * inputs[i];
            }
            hidden[j] = Math.Tanh(sum);        
        }
        double[] output = new double[outputSize];
        for (int k = 0; k < outputSize; k++)
        {
            double sum = 0;
            for (int l = 0; l < hiddenSize; l++)
            {
                sum += weights[(inputs.Length * hiddenSize) + (k * hiddenSize + l)] * hidden[l];
            }
            output[k] = Math.Tanh(sum);
        }

        return output;
    }
    public NeuralNetwork()
    {}
    
}