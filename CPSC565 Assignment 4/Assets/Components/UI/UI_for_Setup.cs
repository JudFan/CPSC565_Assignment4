using UnityEngine;
using UnityEngine.UI;

namespace Assign4.UI 
{
    
    public class UI_for_Setup : MonoBehaviour
    {
        //Fields:
        public int selectedRule;
        public int angle;
        public int numOfPetals;
        public Text output;
        public Text output2;

        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            angle = 0;
            numOfPetals = 0;
            selectedRule = 1;
            output.text = "Angle of Bloom:\n" + angle;
            output2.text = "Number of Petals: " + numOfPetals;
        }

        // Update is called once per frame
        void Update()
        {
            output.text = "Angle of Bloom:\n" + angle;
            output2.text = "Number of Petals: " + numOfPetals;
        }
        public void ListenerMethod(float value)
        {
            //Debug.Log(value);
            angle = (int)value;
        }

        public void ListenerMethod2(float value)
        {
            //Debug.Log(value);
            numOfPetals = (int)value;
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
