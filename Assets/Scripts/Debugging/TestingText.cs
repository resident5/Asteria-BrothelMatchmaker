using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DIALOGUE;

namespace Testing
{


    public class TestingText : MonoBehaviour
    {
        DialogueManager dm;
        TextArchitect architext;
        string[] lines = new string[5]
        {
        "Lorem ipsum odor amet, consectetuer adipiscing elit",
        "Ullamcorper nunc tortor vitae senectus sodales justo nunc",
        "Gravida sit sociosqu non sapien penatibus, aliquam facilisis eget faucibus",
        "Vulputate mauris amet nisl auctor, ornare phasellus fusce",
        "Fusce feugiat maximus porttitor felis malesuada dictum felis orci in",
        };


        private void Start()
        {
            dm = DialogueManager.Instance;
            architext = new TextArchitect(dm.dialogueContainer.dialogueText);
            architext.typeMethod = TextArchitect.TypeMethod.TYPEWRITER;
            architext.Speed = 0.5f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architext.isTyping)
                {
                    if (!architext.textSkip)
                    {
                        architext.textSkip = true;
                    }
                    else
                    {
                        architext.ForceComplete();
                    }
                }
                else
                {
                    architext.Type(lines[Random.Range(0, lines.Length - 1)]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                architext.AppendType(lines[Random.Range(0, lines.Length - 1)]);
            }

        }
    }
}
