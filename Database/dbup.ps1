Push-Location
Set-Location -Path ".\Library.Database.Migrator.Psql"
.\dbup-psql.ps1 
Pop-Location
Push-Location
Set-Location -Path ".\Library.Database.Migrator.Raven"
.\dbup-raven.ps1
Pop-Location