using UnityEngine;
using UnityEngine.UI;

namespace Assign4.UI 
{
    
    public class UI_for_Setup : MonoBehaviour
    {
        //Fields:
        public int selectedRule;
        public int angle;
        public int numOfIters;
        public Text output;
        public Text output2;

        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            angle = 0;
            numOfIters = 0;
            selectedRule = 1;

            if(selectedRule == 1)
            {
                output.text = "Angle of Bloom:\n" + angle;
            }
            else if(selectedRule == 2)
            {
                output.text = "Angle of Branches:\n" + angle;
            }
            
            output2.text = "Number of L-System Iterations: " + numOfIters;
        }

        // Update is called once per frame
        void Update()
        {
            if(selectedRule == 1)
            {
                output.text = "Angle of Bloom:\n" + angle;
            }
            else if(selectedRule == 2)
            {
                output.text = "Angle of Branches:\n" + angle;
            }
            
            output2.text = "Number of L-System Iterations: " + numOfIters;
        }
        public void ListenerMethod(float value)
        {
            //Debug.Log(value);
            angle = (int)value;
        }

        public void ListenerMethod2(float value)
        {
            //Debug.Log(value);
            numOfIters = (int)value;
        }

        public void ListenerMethodForRuleSet(int index)
        {
            //Debug.Log(value);
            if(index == 0)
            {
                selectedRule = 1;
            }
            else if(index == 1)
            {
                selectedRule = 2;
            }
        }
    }
}
