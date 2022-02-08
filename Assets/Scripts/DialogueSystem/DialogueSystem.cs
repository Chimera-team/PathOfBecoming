﻿using System;
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
    private string[][] dialogues;
    Queue<string> linesTriggered = new Queue<string>();//очередь строк, которые триггерятся. именно эта очередь будет выводиться на экран
    private bool isDialogueTyping = false;
    private bool typeDialogeInstantly = false;
    private Dictionary<string, SpeakerObject> avatars = new Dictionary<string, SpeakerObject>();
    [SerializeField] CheckpointDialogue[] checkpoints;
    UnityEvent onComplete;

    private void Awake()
    {
        string dialogFile = Resources.Load<TextAsset>("dialogtest").text;

        dialogFile = Regex.Replace(dialogFile, @"#.*", ""); //@"{(\w*)}","");        
        
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

    public void StartChooseDialogue(bool what)
    {
        choosePanelAnim.SetBool("PanelShow", what);
        SetUI(!what);

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

    //тут
    private IEnumerator TypeLine(string sentence)//написать строку заменив иконки и имена
    {
        if (sentence.Trim() == "" && linesTriggered.Count != 0)
        {
            sentence = linesTriggered.Dequeue();
        }
        else if (linesTriggered.Count == 0)
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
        DialogueTrigger.current = null;
        complete?.Invoke();
        if (Engine.current.playerController.buttonsControl is DialogueHandler)
            Engine.current.playerController.Change_Controls<DefaultHandler>();
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