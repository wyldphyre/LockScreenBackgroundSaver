# LockScreenBackgroundSaver

Looks for the lock screen background images that Windows 10 uses and outputs them to a specified folder. The app is smart enough to deteremine if an image already exists in the output folder, so avoids duplicating images.

## Parameters

- `-shownew`: The output folder will be opened if new images are found
- `-output:<folder>`: The folder where images will be placed

## Ignoring Images

If there are images you don't want placed into the output folder, create an `ignore.txt` file in the output folder and copy the name of the file (excluding the extension) into the file. Place each ignored file name on a separate line. The app will check this file, if present, for files that it will ignore.

## To Do

- Provide a way to specify that only certain types of images are stored, such as desktop or phone backgrounds.
- Look at providing a way to mark images as ignored so that it is possible to keep unwanted images out of the destination folder. (an ignore text file containing the hashes would be the simplest way, I think)
- Provide a way to configure the monitoring frequency (or better yet, just set a sensible fixed value)