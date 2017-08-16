# FSH libary


# Acknowledgment
Special thanks to the authors of the OpenStreetMap wiki page that described the Archive.FSH format. The openstreetmap documentation has served as a general guideline in the creation of the library. 

  https://wiki.openstreetmap.org/wiki/ARCHIVE.FSH

This information has been saved to a pdf that can be found in the Documentation folder for general offline use. 

Please note that the library now supercedes the information contained in the reference document. New information and behaviors have been appropriately commented in the code and better reflects operational needs and capabilities. 


# General design notes
1. All classes inherit from SerializableData.
2. The main entry class is ArchiveFile.
3. The main work methods for SerializableData are the Serialize and Deserialize methods
4. All classes are built to represent the structures defined in the file spec.
5. Only persistence change that is implemented is TrackMetadata.Name with padding unused space with nulls. 
6. The ArchiveFile can be accessed at any time via the static propery Current. 
