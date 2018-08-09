pipeline {
    agent any
    stages {
	    stage('Compiling') {
			steps { 
				echo 'Compiling..'
				sh 'dotnet publish src/NeoPixelServer/NeoPixelServer.csproj --configuration Release --output ../../bin/'
               echo 'Building completed successfully'
			}
		}
		stage('Zip') {
			steps { 
				echo 'Zips the compiled files..'
				sh 'zip -r bin.zip bin'
				echo 'Zip completed successfully'
			}
		}
    }
    post { 
        success { 
            archiveArtifacts artifacts: 'bin.zip', fingerprint: true
        }
    }
}