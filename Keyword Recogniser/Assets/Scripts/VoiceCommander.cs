using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class VoiceCommander : MonoBehaviour
{
    KeywordRecognizer recognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    Stack<int> actionsIndex = new Stack<int>();
    Player player;

    [SerializeField] float timeBetweenActions;
    float timeSinceLastAction;
    // Start is called before the first frame update
    void Start()
    {
        keywords.Add("left", () =>
        {
            actionsIndex.Push(0);
            //player.MoveToTile(new Vector2(-1, 0));
        });

        keywords.Add("right", () =>
        {
            actionsIndex.Push(1);
            //player.MoveToTile(new Vector2(1, 0));
        });

        keywords.Add("Up", () =>
        {
            actionsIndex.Push(2);
            //player.MoveToTile(new Vector2(0, 1));
        });

        keywords.Add("Down", () =>
        {
            actionsIndex.Push(3);
            //player.MoveToTile(new Vector2(0, -1));
        });

        recognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        recognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        recognizer.Start();
    }

    private void Update()
    {
        timeSinceLastAction += Time.deltaTime;

        if(timeSinceLastAction > timeBetweenActions && actionsIndex.Count > 0)
        {
            doAction(actionsIndex.Pop());
            timeSinceLastAction = 0;
        }
    }

    void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;

        if(keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    public void SetPlayer(Player newPlayer)
    {
        player = newPlayer;
    }

    public void doAction(int actionIndex)
    {
        if(actionIndex == 0)
        {
            //player.MoveToTile(new Vector2(-1, 0));
            player.SetDirection(new Vector2(-1, 0));
        }
        else if(actionIndex == 1)
        {
            //player.MoveToTile(new Vector2(1, 0));
            player.SetDirection(new Vector2(1, 0));
        }        
        else if(actionIndex == 2)
        {
            //player.MoveToTile(new Vector2(0, 1));
            player.SetDirection(new Vector2(0, 1));
        }        
        else if(actionIndex == 3)
        {
            //player.MoveToTile(new Vector2(0, -1));
            player.SetDirection(new Vector2(0, -1));
        }
    }
}
