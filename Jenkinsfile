pipeline {
  agent any
  stages {
    stage('Compiling') {
      steps { 
		echo 'Compiling..'
        dotnet publish src/NeoPixelServer/NeoPixelServer.csproj --configuration Release --output ../../bin/
		zip -r bin.zip bin
		echo 'Building completed successfully'
      }
    }
	stage('Zip') {
      steps { 
		echo 'Zips the compiled files..'
        dotnet publish src/NeoPixelServer/NeoPixelServer.csproj --configuration Release --output ../../bin/
		zip -r bin.zip bin
		echo 'Zip completed successfully'
      }
    }
  }
}