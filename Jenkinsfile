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
		stage('Saves Artifacts') {
			steps { 
				echo 'Saves Artifact: bin.zip'
				archiveArtifacts artifacts: 'bin.zip', fingerprint: true

			}
		}
		stage('Deploying') {
			steps { 
				echo 'Stops the old process...'
				sh 'kill $(pgrep dotnet)'
				echo 'Installs the new binaries'
				sh 'unzip bin.zip -d /opt/NeoPixelServer'
			}
		}
    }
}