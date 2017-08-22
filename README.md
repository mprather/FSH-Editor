# The FSH Editor Tool

The FSH Editor is a Windows application to view and edit Raymarine archive.fsh files. You'll also be able to export the data into GPX format and directly view your waypoints and routes using Azure/Bing maps

The project is currently composed of two Visual Studio 2015 projects:

1. FSH.net library - this dll is designed to manage reading and writing archive.fsh files
2. FSH Editor - a WPF executable that utilizes the library 

This toolset was built because we wanted to interact with the data stored in our Raymarine E-120W chartplottters. 

## Downloading 
If you would like to download the tool without fussing with code, we've made the tool available at our boat's web site.

[Download FSH Editor](http://www.okeanvoyaging.com/fsh-editor-download)


## General documentation
We've also posted a simple 1-page help file. The [FSH Editor Help and General Information](http://www.okeanvoyaging.com/fsh-editor-help-and-general-information) page will be updated regularly as the toolset is improved.


## Author
FSH Editor and the FSH.net library is developed and maintained by Maurice Prather.

## Misc
The project is written in C# and is a direct interpretation of the documentation posted on the OpenStreetMap wiki page that describes the [Archive.FSH](https://wiki.openstreetmap.org/wiki/ARCHIVE.FSH) format. The FSH library is currently more accurate than the format outlined in the wiki page. 

## License
FSH Editor is released under GNU GPLv3.
