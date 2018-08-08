pipeline {
  agent any
  stages {
    stage('Compiling') {
      steps { 
		echo 'Compiling..'
        sh './build.sh'
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
}