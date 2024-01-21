Get-ChildItem .\ -include bin,obj,packages,.vs -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }
