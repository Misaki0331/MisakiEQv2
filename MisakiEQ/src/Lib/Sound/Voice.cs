using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace MisakiEQ.src.Lib.Sound
{
    internal class Voice
    {
        static void VoiceMain(string[] args)
        {

            // Initialize a new instance of the SpeechSynthesizer.  
            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output.   
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.  
            synth.Speak("This example demonstrates a basic use of Speech Synthesizer");

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
