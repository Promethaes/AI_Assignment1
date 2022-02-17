//Anthony Smiderle
//100695532
//2022/02/07
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] FileIO file = null;
    [SerializeField] Door doorPrefab = null;
    Dictionary<string, float> diction = new Dictionary<string, float>();

    // Start is called before the first frame update
    void Start()
    {
        //read in probabilities
        var input = file.ReadInProbabilities();
        var strings = input.Split('\n');

        //split the probabilities into string to float dictionary
        for (int i = 0; i < strings.Length - 1; i++)
        {
            var s = strings[i].Split(' ');
            diction[s[0]] = float.Parse(s[1]);
        }

        //dictionary for storing the number of times each door type is picked
        Dictionary<string, int> choices = new Dictionary<string, int>();
        foreach (var d in diction)
            choices[d.Key] = 0;

        for (int i = 0; i < 20; i++)
        {
            float rand = Random.Range(0.0f, 100.0f);
            string choice = "";
            foreach (var d in diction.OrderByDescending((a) => a.Value))
            {
                //if the random number is within the chance range, set the choice to this type
                if (rand <= d.Value * 100.0f)
                    choice = d.Key;
                else
                    break;

            }
            if (choice == "")
            {
                i--;
                continue;
            }
            //add one door of this type
            choices[choice]++;
        }

        //instantiate the doors
        List<GameObject> doors = new List<GameObject>();
        int counter = 0;
        foreach (var c in choices.OrderByDescending((a) => a.Value))
        {
            bool hot = ParseCharacter(c.Key[0]);
            bool noisy = ParseCharacter(c.Key[1]);
            bool safe = ParseCharacter(c.Key[2]);

            for (int i = 0; i < c.Value; i++)
            {
                var door = GameObject.Instantiate(doorPrefab.gameObject, new Vector3(counter * 4.0f, 0.0f, 11110.0f), Quaternion.Euler(0.0f,180.0f,0.0f));
                door.GetComponent<Door>().SetHotNoisySafe(hot, noisy, safe);
                doors.Add(door);
                counter++;
            }
        }

        //place the doors randomly
        float mag = doors[0].transform.position.magnitude;
        for (int i = 0; i < 20; i++)
        {
            int index = Random.Range(0, doors.Count);
            if (doors[index].transform.position.magnitude <= 80.0f)
            {
                i--;
                continue;
            }
            doors[index].transform.position = new Vector3(i * 4.0f, 0.5f, 0.0f);
            doors.RemoveAt(index);
        }

        //Debug.Log(counter);

    }

    //helper to parse char to bool
    bool ParseCharacter(char choice)
    {
        return choice == 'Y' ? true : false;
    }
}
