using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Timeline;
using System.Text.RegularExpressions;
using TMPro;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;

public class ComboDetector : MonoBehaviour
{
    [Serializable]
    public class ComboEntry
    {
        public string comboName;
        public List<char> sequence;
        public string scene;
        public string advance;
        public float duration;
    }
    [SerializeField]
    private List<ComboEntry> combos = new List<ComboEntry>
    {
        new ComboEntry {comboName = "OFFICECOMBO", sequence = new List<char>{'L','R','L','R','U'}, scene = "OFFICE", advance = "STREET", duration = 5f},
        new ComboEntry {comboName = "STREETCOMBO", sequence = new List<char>{'U','U','R','R','R'}, scene = "STREET", advance = "GRASS", duration = 5f},
        new ComboEntry {comboName = "GRASSCOMBO", sequence = new List<char>{'R','L','R'}, scene = "GRASS", advance = "SKY", duration = 3f},
        new ComboEntry {comboName = "SKYCOMBO", sequence = new List<char>{'R','R','R','R','R','R'}, scene = "SKY", advance = "DICTOFF", duration = 6f},
        new ComboEntry {comboName = "DICTOFFCOMBO", sequence = new List<char>{'R', 'R','R','R','R','R'}, scene = "DICTOFF", advance = "SPACE", duration = 6f},
        new ComboEntry {comboName = "SPACECOMBO", sequence = new List<char>{'R', 'R', 'R','R','R','R'}, scene = "SPACE", advance = "EARTH", duration = 6f},
        new ComboEntry {comboName = "EARTHCOMBO", sequence = new List<char>{'R','R','R','R','R','R'}, scene = "EARTH", advance = "OFFICE", duration = 6f},
    };

    [SerializeField] private VisualController visualController;
    [SerializeField] private int maxBufferLength = 10;
    [SerializeField] private TextMeshProUGUI cstat;
    [SerializeField] private TextMeshProUGUI tstat;
    [SerializeField] private UnityEngine.UI.Image tout;

    private List<char> inputBuffer = new List<char>();
    private float lastInputTime;
    private int currCombo = 0;
    private bool canInput = false;
    private float inputTimeout = 1.5f;
    

    void Start()
    {
        Debug.Log($"Loading Combo: {combos[currCombo].comboName}");
        canInput = true;
        inputTimeout = combos[0].duration;
        lastInputTime = Time.time;
        visualController.ShowKey(combos[currCombo].sequence[0]);
        foreach(var c in combos)
        {
            maxBufferLength = Mathf.Max(maxBufferLength, c.sequence.Count);
        }
    }
    void Update()
    {
        if (Keyboard.current == null) return;
        if (canInput && Time.time - lastInputTime > inputTimeout)
        {
            StartCoroutine(InvalidCombo());
        }

        char? pressedKey = GetDirectionalKeyPressed();
        if (pressedKey.HasValue && canInput)
        {
            var expected = combos[currCombo].sequence;
            if (inputBuffer.Count < expected.Count && pressedKey.Value == expected[inputBuffer.Count])
            {
                inputBuffer.Add(pressedKey.Value);
                //lastInputTime = Time.time;
                
                int nextIn = inputBuffer.Count;
                if (nextIn < expected.Count)
                {
                    visualController.AdvanceKey(expected[nextIn]);
                }
                if (inputBuffer.Count > maxBufferLength)
                {
                    inputBuffer.RemoveAt(0);
                } 
            } 
            else
            {
                inputBuffer.Add(pressedKey.Value);
                StartCoroutine(InvalidCombo());
            } 
        }
        CheckCombo();
        if (cstat != null && tstat != null)
        {
            cstat.text = $"Combo: [{string.Join(", ", inputBuffer)}] {inputBuffer.Count}";
            tstat.text = $"Target: [{string.Join(", ", combos[currCombo].sequence)}]";
        }
        UpdateTimeout();
    }

    char? GetDirectionalKeyPressed()
    {
        var kb = Keyboard.current;
        if (kb.upArrowKey.wasReleasedThisFrame || kb.wKey.wasPressedThisFrame) return 'U';
        if (kb.downArrowKey.wasReleasedThisFrame || kb.sKey.wasPressedThisFrame) return 'D';
        if (kb.leftArrowKey.wasReleasedThisFrame || kb.aKey.wasPressedThisFrame) return 'L';
        if (kb.rightArrowKey.wasReleasedThisFrame || kb.dKey.wasPressedThisFrame) return 'R';
        return null;
    }
    void CheckCombo()
    {
        if (currCombo >= combos.Count) return;
        var combo = combos[currCombo];
        if (TailMatch(combo.sequence) && canInput)
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
        StartCoroutine(ValidCombo());

        if (!string.IsNullOrEmpty(combo.advance))
        {
            //SceneManager.LoadScene(combo.sceneToLoad);
        }
    }
    IEnumerator ValidCombo()
    {
        canInput = false;
        cstat.color = Color.green;
        tstat.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        cstat.color = Color.white;
        tstat.color = Color.white;
        inputBuffer.Clear();
        currCombo = (currCombo + 1) % combos.Count;
        inputTimeout = combos[currCombo].duration;
        lastInputTime = Time.time;
        visualController.ShowKey(combos[currCombo].sequence[0]);
        canInput = true;
        Debug.Log($"combo dur: {inputTimeout}");
    }
    IEnumerator InvalidCombo()
    {
        Debug.Log($"Combo Failed: {combos[currCombo].comboName} -> loading '{combos[0].comboName}");
        canInput = false;
        cstat.color = Color.red;
        tstat.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        cstat.color = Color.white;
        tstat.color = Color.white;
        inputBuffer.Clear();
        currCombo = 0;
        inputTimeout = combos[currCombo].duration;
        lastInputTime = Time.time;
        canInput = true;
        Debug.Log($"combo dur: {inputTimeout}");
    }

    void UpdateTimeout()
    {
        if (tout == null) return;
        if (canInput)
        {
            float elapsed = Time.time - lastInputTime;
            float remaining = Mathf.Clamp01(1f - (elapsed/inputTimeout));
            tout.fillAmount = remaining; 
        }
        else
        {
            tout.fillAmount = 1;
        }
        
    }
}
