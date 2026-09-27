using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Timeline;
using System.Text.RegularExpressions;

public class ComboDetector : MonoBehaviour
{
    [Serializable]
    public class ComboEntry
    {
        public string comboName;
        public List<char> sequence;
        public string advance;
    }
    [SerializeField]
    private List<ComboEntry> combos = new List<ComboEntry>
    {
        new ComboEntry {comboName = "OFFICECOMBO", sequence = new List<char>{'L','R','L','R','U'}, advance = "STREET"},
        new ComboEntry {comboName = "STREETCOMBO", sequence = new List<char>{'U','U','R','R','R'}, advance = "GRASS"},
        new ComboEntry {comboName = "GRASSCOMBO", sequence = new List<char>{'R','R','R','R','R','R'}, advance = "SKY"},
        new ComboEntry {comboName = "SKYCOMBO", sequence = new List<char>{'R','R','R','R','R','R'}, advance = "DICTOFF"},
        new ComboEntry {comboName = "DICTOFFCOMBO", sequence = new List<char>{'R','R','R','R','R','R'}, advance = "SPACE"},
        new ComboEntry {comboName = "SPACECOMBO", sequence = new List<char>{'R','R','R','R','R','R'}, advance = "EARTH"},
        new ComboEntry {comboName = "EARTHCOMBO", sequence = new List<char>{'R','R','R','R','R','R'}, advance = "OFFICE"},
    };

    [SerializeField] private float inputTimeout = 1.5f;
    [SerializeField] private int maxBufferLength = 8;

    private List<char> inputBuffer = new List<char>();
    private float lastInputTime;
    private int currCombo = 0;

    void Start()
    {
        Debug.Log($"Loading Combo: {combos[currCombo].comboName}");
        foreach(var c in combos)
        {
            maxBufferLength = Mathf.Max(maxBufferLength, c.sequence.Count);
        }
    }
    void Update()
    {
        if (Keyboard.current == null) return;
        if (inputBuffer.Count > 0 && Time.time - lastInputTime > inputTimeout)
        {
            inputBuffer.Clear();
        }

        char? pressedKey = GetDirectionalKeyPressed();
        if (pressedKey.HasValue)
        {
            Debug.Log(pressedKey);
            inputBuffer.Add(pressedKey.Value);
            lastInputTime = Time.time;

            if (inputBuffer.Count > maxBufferLength)
            {
                inputBuffer.RemoveAt(0);
            }
        }
        CheckCombo();
    }

    char? GetDirectionalKeyPressed()
    {
        var kb = Keyboard.current;
        if (kb.upArrowKey.wasReleasedThisFrame) return 'U';
        if (kb.downArrowKey.wasReleasedThisFrame) return 'D';
        if (kb.leftArrowKey.wasReleasedThisFrame) return 'L';
        if (kb.rightArrowKey.wasReleasedThisFrame) return 'R';
        return null;
    }
    void CheckCombo()
    {
        if (currCombo >= combos.Count) return;
        var combo = combos[currCombo];
        if (TailMatch(combo.sequence))
        {
            OnComboSuccess(combo);
        }
    }
    bool TailMatch(List<char> sequence)
    {
        if (sequence.Count == 0) return false;
        if (inputBuffer.Count < sequence.Count) return false;
        int offset = inputBuffer.Count - sequence.Count;
        for (int i = 0; i < sequence.Count; i++){
            if (inputBuffer[offset + i] != sequence[i]) return false;
        }
        return true;
    }

    void OnComboSuccess(ComboEntry combo)
    {
        Debug.Log($"Combo matched: {combo.comboName} -> loading '{combo.advance}'");
        inputBuffer.Clear();

        if (!string.IsNullOrEmpty(combo.advance))
        {
            //SceneManager.LoadScene(combo.sceneToLoad);
        }
        currCombo = (currCombo + 1) % combos.Count;
    }
}
