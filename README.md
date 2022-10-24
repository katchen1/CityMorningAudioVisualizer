# CityMorningAudioVisualizer
An audio visualizer inspired by [the music video of Yoasobi’s Gunjo](https://youtu.be/Y4nEEZwckuU), a song that inspires listeners to immerse themselves in what they like and express what they see. Produced with Unity+Chuck (Chunity) in October 2022.
## Screenshots
![image9](https://user-images.githubusercontent.com/59420335/197607583-628a5a75-0017-4f38-9d19-7842e8446a46.png)
![image7](https://user-images.githubusercontent.com/59420335/197607605-d5878cf9-8765-4a4b-b1fb-6e7836ca0947.png)
![image12](https://user-images.githubusercontent.com/59420335/197607616-d9d36f00-fae6-4a76-b723-ddd6f07d5e3c.png)
![image13](https://user-images.githubusercontent.com/59420335/197607622-51329028-9513-4f82-ae4a-3c060f0dbf11.png)

## Video
https://youtu.be/ZKEEDSzcTjU

## Production build (MacOS platform)
https://drive.google.com/drive/folders/1CSMG_LGvitRcoeBXbVkFT1iCPyvEF8GK?usp=sharing 

## Instructions for using the audio visualizer
* Platform: MacOS
1. Download the build file and save it to a local folder.
2. Right click on the file → click “Open”. Allow it to use your microphone.
  * If you run into the “application cannot be opened” error, set the executable flag by running “chmod -R +x <app name>.app/Contents/MacOS” in the terminal, then try opening the file again.
3. Let the narrative play (for about 75 seconds).
4. Press 1/2/3 number keys to toggle between the 3 main camera views and experiment with the different spectrum/waveform visualizations using your microphone.

| View (keyboard input) | Where/how is the spectrum history visualized? | Where/how is the waveform visualized? |
| ----------------------| --------------------------------------------- | --------------------------------------|
| Car (1) | Circles (raindrops) on the window | 1) The red wave along the windowsill, 2) Vertical movement of the car | 
| City (2)   | The road surface | Red wires between the poles |
| Sky (3) | Circles around the rotating planet | Stars expand/shrink according to the average of the absolute value of each data point in the waveform | 

## Comments on constructing the audio visualizer and difficulties encountered
I started this project with the idea of using the audiovisual waves to represent earthquake waves. From here, I started making an earth object and stars and planets around the earth in Unity. Then, I took a turn and decided to play with perspective utilizing camera panning and zooming. I thought I could zoom into a city then zoom out to the entire earth, capturing micro and macro motion with the spectrum history and time domains. The most difficult part was brainstorming ideas on what in the city or what on the earth can be aesthetically represented by the waves, which eventually led to the moving car, raindrop ripples, expanding stars, etc. Next, I played with moving the camera in a way that matches the dynamics of the music. Finally, I edited the music with Chunity and added a low pass filter and a high pass filter, each set to a different random frequency once every few seconds, to make the music sound more unnatural.

## Acknowledgements
The FA22_Music256A_CS476A discord chat. I ran into issues such as running into build errors, and my classmates who encountered the same issues helped me out.
