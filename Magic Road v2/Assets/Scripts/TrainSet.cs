using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;




    public class TrainSet
    {
        public int INPUT_SIZE;
        public int OUTPUT_SIZE;

        private List<double[][]> data = new List<double[][]>();

        public TrainSet(int INPUT_SIZE, int OUTPUT_SIZE)
        {
            this.INPUT_SIZE = INPUT_SIZE;
            this.OUTPUT_SIZE = OUTPUT_SIZE;
        }
        public void addData(double[] i, double[] expected)
        {
            if (i.Length != INPUT_SIZE || expected.Length != OUTPUT_SIZE) return;
            data.Add(new double[][] { i, expected });
        }
        public TrainSet extractBatch(int size)
        {
            if (size > 0 && size <= this.size())
            {
                TrainSet set = new TrainSet(INPUT_SIZE, OUTPUT_SIZE);
                int[] ids = NetworkTools.randomValues(0, this.size() - 1, size);
                foreach (int i in ids)
                {
                    set.addData(this.getInput(i), this.getOutput(i));
                }
                return set;
            }
            else return this;
        }

        public double[] getInput(int index)
        {
            if (index >= 0 && index < size())
                return data[index][0];
            else return null;
        }
        public double[] getOutput(int index)
        {
            if (index >= 0 && index < size())
                return data[index][1];
            else return null;
        }
        public int getINPUT_SIZE() { return INPUT_SIZE; }
        public int getOUTPUT_SIZE() { return OUTPUT_SIZE; }
        public int size() { return data.Count; }

    }


