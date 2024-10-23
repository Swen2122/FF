using UnityEngine;
using UnityEngine.Audio;

public static class PitchChanger
{
    // ����� pitch �� ����� ��������
    public static void ChangePitch(AudioMixer audioMixer, ref float pitch, float change)
    {
        pitch += change;
        pitch = Mathf.Clamp(pitch, 0.75f, 1.25f);  // �������� pitch �� 75% �� 125%

        // ������������ �������� pitch � Audio Mixer
        audioMixer.SetFloat("Pitch", pitch);
    }

    // ��������� ���� pitch
    public static void RandomChangePitch(AudioMixer audioMixer, ref float pitch)
    {
        float[] pitchChanges = { -0.15f, -0.10f, -0.05f, 0.05f, 0.10f, 0.15f };
        int randomIndex = Random.Range(0, pitchChanges.Length);
        pitch += pitchChanges[randomIndex];

        pitch = Mathf.Clamp(pitch, pitch - 0.25f, pitch + 0.25f);  // �������� pitch

        // ������������ ���� �������� pitch � Audio Mixer
        audioMixer.SetFloat("Pitch", pitch);
    }
}