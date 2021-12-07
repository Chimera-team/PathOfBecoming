using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpeakerObject
{
    private Sprite avatarSprite;
    private string name;

    public Sprite AvatarSprite { get => avatarSprite; set => avatarSprite = value; }
    public string Name { get => name; set => name = value; }

    public SpeakerObject(Sprite sprite, string name)
    {
        avatarSprite = sprite;
        this.name = name;
    }
}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] Text output;//ссылка на текст который будет на UI отображаться
    [SerializeField] Text nameOutput;//ссылка на имя которое будет на UI отображаться
    [SerializeField] Animator panelAnim;//анимация панели диалогов
    [SerializeField] Animator choosePanelAnim;// анимация панели выбора диалогов
    [SerializeField] Image dialogueImg;//ссылка на картинку говорящего персонажа
    [SerializeField] Sprite henryImg;//ссылка непосредственно на спрайт игрока
    [SerializeField] Sprite fairyImg;//ссылка непосредственно на спрайт феи
    [SerializeField] Sprite impImg;//ссылка непосредственно на спрайт анчутки
    [SerializeField] Sprite catImg;//на спрайт кота

<<<<<<< Updated upstream
    public string[] DialoguesFile;//все строки файла
=======
    [SerializeField] GameObject[] chooseButtons;
    [SerializeField] RectTransform pointer;
    private int currentPointerPosition;

>>>>>>> Stashed changes
    private string[][] dialogues;
    Queue<string> linesTriggered = new Queue<string>();//очередь строк, которые триггерятся. именно эта очередь будет выводиться на экран
    private bool isDialogueTyping = false;
    private bool typeDialogeInstantly = false;
    private Dictionary<string, SpeakerObject> avatars = new Dictionary<string, SpeakerObject>();
    [SerializeField] CheckpointDialogue[] checkpoints;
    UnityEvent onComplete;

    private void Awake()
    {
        DialoguesFile = FileParser("Russian2", '\n');

        string dialogFile = Resources.Load<TextAsset>("Russian2").text;

        dialogFile = Regex.Replace(dialogFile, @"{(\w*)}","");        
        
        Dictionary<string, string> dialogueNames = new Dictionary<string, string>();
        string[] names = FileParser("DialogueNames", '\n');
        foreach (string name in names)
        {
            dialogueNames.Add(name.Split('=')[0].Trim(), name.Split('=')[1].Trim());
        }
        foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/Avatars"))
        {
            avatars.Add(sprite.name, new SpeakerObject(sprite, dialogueNames[sprite.name]));
        }

        string[] dialogTMP = TextParser(dialogFile, '*');
        dialogues = new string[dialogTMP.Length][];
        for (int i = 0; i < dialogTMP.Length; i++)
        {
            dialogues[i] = dialogTMP[i].Replace("\r",""). Split('\n');
        }
    }

    public void Next()//метод для пропуска строчки
    {
        DisplayNextLine();
    }
    public void SetUI(bool what)//метод отключающий лишний UI
    {
        Interface.current.Enable_Interface(what);
    }

    public void StartDialogue(int dialogNum, UnityEvent onComplete)
    {
        this.onComplete = onComplete;
        panelAnim.SetBool("PanelShow", true);
        SetUI(false);
        foreach (string str in dialogues[dialogNum])
        {
            linesTriggered.Enqueue(str);
        }
        DisplayNextLine();
    }
    //тут
    public void StartDialogue(int startLine, int endLine, UnityEvent onComplete)//неачать диалог
    {
        this.onComplete = onComplete;
        panelAnim.SetBool("PanelShow", true);
        SetUI(false);
        for (int i = startLine; i < endLine; i++)
        {
            linesTriggered.Enqueue(DialoguesFile[i]);
        }
        DisplayNextLine();
    }

    public void StartDialogue(Dialogue dialogue, UnityEvent onComplete)
    {
        StartDialogue(dialogue.startStringId, dialogue.endStringId, onComplete);
    }

    public void DisplayNextLine()//показать следущую строку
    {
        if (!isDialogueTyping)
        {
            if (linesTriggered.Count == 0)
            {
                EndDialogue();
                return;
            }
            string line = linesTriggered.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeLine(line));
        }
        else
        {
            typeDialogeInstantly = true;
        }
    }

    public void StartChooseDialogue(int[] dialogueNumbers)
    {
         for(int i = 0;i<dialogueNumbers.Length;i++)
        {
            chooseButtons[i].SetActive(true);
        }
        pointer.position = new Vector3(pointer.rect.x, chooseButtons[0].transform.position.y);
        currentPointerPosition = 0;
        StartCoroutine(ChoosingDialogues(dialogueNumbers));
    }

    private IEnumerator ChoosingDialogues(int[] dialogueNumbers)
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            if(currentPointerPosition<dialogueNumbers.Length-1)
            {
                currentPointerPosition++;
            }
            
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            if(currentPointerPosition>0)
            {
                currentPointerPosition--;
            }
        }
        pointer.position = new Vector3(pointer.rect.x, chooseButtons[currentPointerPosition].transform.position.y);
        if (Input.GetKeyDown(KeyCode.E))
        {

            StartDialogue(currentPointerPosition, onComplete);
            yield break;
        }
        yield return null;
    }
    
    private IEnumerator TypeLine(string sentence)//написать строку заменив иконки и имена
    {
        if(sentence.Trim() == "")
        {
            yield break;
        }
        string nameString = sentence.Split('=')[0].Trim();
        if(avatars.ContainsKey(nameString))
        {
            nameOutput.text = avatars[nameString].Name;
            dialogueImg.sprite = avatars[nameString].AvatarSprite;
        }
        else
        {
            Debug.LogWarning("НЕ НАЙДЕНО В СЛОВАРЕ АВАТАРОВ");
        }

        string result = sentence.Substring(sentence.IndexOf('=') + 1);
        sentence = result.Trim();
        output.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            isDialogueTyping = true;
            if (!typeDialogeInstantly)
            {
                yield return new WaitForSecondsRealtime(0.025f);
                output.text += letter;
            }
            else
            {
                output.text = sentence;
                break;
            }
        }
        isDialogueTyping = false;
        typeDialogeInstantly = false;
    }
    public void EndDialogue()//закончить диалог 
    {
        panelAnim.SetBool("PanelShow", false);
        SetUI(true);
        UnityEvent complete = onComplete;
        complete?.Invoke();
    }

    public void Checkpoint(CheckpointDialogue dialogue)
    {
        for (int i = 0; i < checkpoints.Length; i++)
            if (dialogue == checkpoints[i])
            {
                Engine.current.Checkpoint(i);
                break;
            }
    }

    public void Load_State(int index)
    {
        for (int i = 0; i <= index; i++)
            checkpoints[i].onCheckpoint?.Invoke();
        checkpoints[index].onTrigger?.Invoke();
    }
    private string[] FileParser(string fileName, char param)
    {
        TextAsset file = Resources.Load<TextAsset>(fileName);
        string[] parsed = file.text.Split(param);
        return parsed;
    }
    private string[] TextParser(string text, char param)
    {
        string[] parsed = text.Split(param);
        return parsed;
    }
}