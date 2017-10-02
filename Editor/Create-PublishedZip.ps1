$logFile = "log.txt"
$zipFile = "$((gl).ProviderPath)\FSH_Editor_v1.5.zip"


# =============================================================================
# Define functions
# =============================================================================
function Add-Zip {
  
  param(
    [string]$filename = $(throw "Please specify a file name for the zip file")
  )
  
  ## Found via http://blogs.msdn.com/b/daiken/archive/2007/02/12/compress-files-with-windows-powershell-then-package-a-windows-vista-sidebar-gadget.aspx
  
  $old = $host.ui.rawui.ForegroundColor
  $host.ui.rawui.ForegroundColor = "Yellow"
  
  WriteInfo "Processing zip file: $filename"
  
  $host.ui.rawui.ForegroundColor = $old    
  
  if(-not (test-path($filename))) {
    
    WriteInfo "Creating new zip file"
    
    set-content $filename ("PK" + [char]5 + [char]6 + ("$([char]0)" * 18))
    (dir $filename).IsReadOnly = $false
    
  }
  
  $shellApplication = new-object -com shell.application
  $zipPackage = $shellApplication.NameSpace($filename)
  
  WriteInfo "Updating zip file"
  
  # NOTE: Expecting a FileInfo object for input; however, input is handed over as an enumerator
    
  foreach ($f in $input) { 
      
    #WriteInfo "  Adding $($f.Name) " -NoNewline
    
    $zipPackage.CopyHere($f.FullName)
    
    while($zipPackage.Items().Item($f.Name) -Eq $null) {
      # Pause the process to allow the async process to catch up a bit
      sleep -milliseconds 500
    }

  }
  
}  # End of Add-Zip

function CreateArchive([System.IO.FileSystemInfo] $file) {
  
  WriteInfo "Processing: $($file.FullName)"
  
  $name = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
  
  # Create or update the zip file by adding the file...
  $file | Add-Zip $zipFile

  # Turn off the archive flag...
  $file.Attributes = $file.Attributes -bxor [System.IO.FileAttributes]::Archive
  
  # WriteInfo "Archiving zip file"
  
}  # End of CreateArchive

function WriteInfo([string] $text, [switch] $createNew) {
  if ($createNew) {
    "$(date)| " + $text | tee-object -FilePath $logFile
  } else {
    "$(date)| " + $text | tee-object -FilePath $logFile -Append
  }
}

# =============================================================================
# Main script
# =============================================================================

# Set the current directory so that we have a baseline folder reference for 
# other calls to System.IO.Directory

WriteInfo "Starting" -createNew
[System.IO.Directory]::SetCurrentDirectory("$((gl).ProviderPath)")

if (Test-Path $zipfile) {
  del $zipfile
}

[System.IO.FileSystemInfo[]]$files = dir "FSH Editor.exe*"

$files += dir "FSH.Library.dll"

$files | % { 
  CreateArchive $_ 
}
