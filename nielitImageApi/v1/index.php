<?php
/**
 * NIELIT Image Validation Api Business logic code 
 *
 * @author Danish Akhtar
 * 
 */
require '.././libs/Slim/Slim.php';

\Slim\Slim::registerAutoloader();

$app = new \Slim\Slim();

/**
 * Adding Middle Layer to authenticate every request
 * Checking if the request has valid api key in the 'Authorization' header
 */
function authenticate(\Slim\Route $route) {
    // Getting request headers
    $headers = apache_request_headers();
    $response = array();
    $app = \Slim\Slim::getInstance();

    // Verifying Authorization Header
    if (isset($headers['Authorization'])) {
       // get the api key
        $api_key = $headers['Authorization'];
        // validating api key
        if ($api_key != 'Test@123') {
            // api key is not present in users table
            $response["error"] = true;
            $response["message"] = "Access Denied. Invalid Api key";
            echoRespnse(401, $response);
            $app->stop();
        }
    } else {
        // api key is missing in header
        $response["error"] = true;
        $response["message"] = "Api key is misssing";
        echoRespnse(400, $response);
        $app->stop();
    }
}




/**
 * ----------- METHODS WITHOUT AUTHENTICATION ---------------------------------
 */
/**
 * Image Validation 
 * url - /ImageValidate
 * method - POST
 * params - image
 */
$app->post('/ImageValidate', 'authenticate', function() use ($app) {
   
    $response = array();
	
	if (!empty($_FILES["image"]["name"])) {

        $allowed_image_extension = array("jpg","jpeg","JPEG","JPG");
        // Get image file extension
        $file_extension = pathinfo($_FILES["image"]["name"], PATHINFO_EXTENSION); 

       if (! in_array($file_extension, $allowed_image_extension)) {

                $response["error"] = true;
                $response['status'] = FALSE;
                $response["message"] = "Upload valid images. Only JPG and JPEG are allowed.";
           
        
        } else {


		$img = new imagick($_FILES["image"]["tmp_name"]);
        $dpi = $img->getImageResolution();


        $mime = $img->getImageMimeType(); 
		
		// Get Image Dimension
		$fileinfo = @getimagesize($_FILES["image"]["tmp_name"]);
		$width = $fileinfo[0];
		$height = $fileinfo[1];

		$allowed_image_extension = array("jpg","jpeg","JPEG","JPG");
		$allowed_image_mime = array("image/x-jpeg","image/jpeg","image/x-jpg","image/x-jpg");
        // Get image file extension
        $file_extension = pathinfo($_FILES["image"]["name"], PATHINFO_EXTENSION);


         // Validate file input to check if is not empty
    	if (! file_exists($_FILES["image"]["tmp_name"])) {
 
            $response["error"] = true;
            $response['status'] = FALSE;
            $response["message"] = "Choose image file to upload.";
        	
    	// Validate file input to check if is with valid extension
    	}   else if (! in_array($file_extension, $allowed_image_extension)) {
      		
            $response["error"] = true;
            $response['status'] = FALSE;
            $response["message"] = "Upload valid images. Only JPG and JPEG are allowed.";
           

        // Validate image MIME type
    	}   else if (! in_array($mime, $allowed_image_mime)) {

            $response["error"] = true;
            $response['status'] = FALSE;
            $response["message"] = "Image Mime invalid.Image MIME should be image/x-jpeg, image/jpeg, image/x-jpg, image/x-jpg. Your current image MIME is $mime";
      		
        // Validate image file size
    	}   else if (($_FILES["image"]["size"] < 5120)) {
    		
    		$current_size = $_FILES["image"]["size"] / 1024;
    		$int_cast = (int)$current_size; 
        	
            $response["error"] = true;
            $response['status'] = FALSE;
            $response["message"] = "Image size should be greater than 5kb your current image size is $int_cast KB";

        	
        }  else if (($_FILES["image"]["size"] > 52224)) {
            $current_size = $_FILES["image"]["size"] / 1024;
            $int_cast = (int)$current_size; 

            $response["error"] = true;
            $response['status'] = FALSE;
            $response["message"] = "Image size should be less than and equal to 50kb your current image size is $int_cast KB";
            
        } else if($width == $height) {
    	
            $response["error"] = true;
            $response['status'] = FALSE;
            $response["message"] = "Square image not allowed";
		} else if(!(($width == 132) && ($height == 170))) {
		//} else if(!(($width > 166 && $width < 201) && ($height > 249 && $height < 301))) {
    	//} else if(!(($width > 131 && $width < 251) && ($height > 169 && $height < 301))) {
            $response["error"] = true;
            $response['status'] = FALSE;
            //$response["message"] = "Image width should be within 132px to 250px and height 170 px to 300  px your current dimensions are $width px X $height px";
			$response["message"] = "Image width should be 132px and height 170 px your current dimensions are $width px X $height px";
		
		} else if(!(($dpi['x'] > 95 && $dpi['x'] < 301) && ($dpi['y'] > 95 && $dpi['y'] < 301))) {
			
            $response["error"] = true;
            $response['status'] = FALSE;
            $response["message"] = "Image Resolution should be greater than 96 dpi and less than 300 dpi your current image horizontal resolution is ".$dpi['x']." dpi and vertical resolution is ".$dpi['y']." dpi";

		} else {
            include "FaceDetector.php";
            $face_detect = new svay\FaceDetector('detection.dat');
		    $face = $face_detect->faceDetect($_FILES["image"]["tmp_name"]);
            if ($face) {
                $response["error"] = true;
                $response['status'] = TRUE;
                $response["message"] = "Image validated successfully";
			} else {
                $response["error"] = false;
                $response['status'] = FALSE;
                $response["message"] = "Invalid image found.Facial features not clearly visible. Please try again!";
			}


        }  
    }
	} else {

        $response["error"] = true;
        $response['status'] = FALSE;
        $response["message"] = "image parameter should not be blank. Please try again!";

	}
    // echo json response
    echoRespnse(200, $response);
});

/**
 * Thumb and Signature Validation 
 * url - /ThumbSignatureValidate
 * method - POST
 * params - image
 */
$app->post('/ThumbSignatureValidate', 'authenticate', function() use ($app) {
   
    $response = array();
	
	if (!empty($_FILES["image"]["name"])) {

        $allowed_image_extension = array("jpg","jpeg","JPEG","JPG");
        // Get image file extension
        $file_extension = pathinfo($_FILES["image"]["name"], PATHINFO_EXTENSION); 

       if (! in_array($file_extension, $allowed_image_extension)) {

               
                $response['status'] = FALSE;
                $response["message"] = "Upload valid images. Only JPG and JPEG are allowed.";
           
        
        } else {


		$img = new imagick($_FILES["image"]["tmp_name"]);
        $dpi = $img->getImageResolution();


        $mime = $img->getImageMimeType(); 
		
		// Get Image Dimension
		$fileinfo = @getimagesize($_FILES["image"]["tmp_name"]);
		$width = $fileinfo[0];
		$height = $fileinfo[1];

		$allowed_image_extension = array("jpg","jpeg","JPEG","JPG");
		$allowed_image_mime = array("image/x-jpeg","image/jpeg","image/x-jpg","image/x-jpg");
        // Get image file extension
        $file_extension = pathinfo($_FILES["image"]["name"], PATHINFO_EXTENSION);


         // Validate file input to check if is not empty
    	if (! file_exists($_FILES["image"]["tmp_name"])) {
 
            
            $response['status'] = FALSE;
            $response["message"] = "Choose image file to upload.";
        	
    	// Validate file input to check if is with valid extension
    	}   else if (! in_array($file_extension, $allowed_image_extension)) {
      		
            
            $response['status'] = FALSE;
            $response["message"] = "Upload valid images. Only JPG and JPEG are allowed.";
           

        // Validate image MIME type
    	}   else if (! in_array($mime, $allowed_image_mime)) {

            
            $response['status'] = FALSE;
            $response["message"] = "Image Mime invalid.Image MIME should be image/x-jpeg, image/jpeg, image/x-jpg, image/x-jpg. Your current image MIME is $mime";
      		
        // Validate image file size
    	}   else if (($_FILES["image"]["size"] < 5120)) {
    		
    		$current_size = $_FILES["image"]["size"] / 1024;
    		$int_cast = (int)$current_size; 
        	
            
            $response['status'] = FALSE;
            $response["message"] = "Image size should be greater than 5kb your current image size is $int_cast KB";

        	
        }  else if (($_FILES["image"]["size"] > 20480)) {
            $current_size = $_FILES["image"]["size"] / 1024;
            $int_cast = (int)$current_size; 

           
            $response['status'] = FALSE;
            $response["message"] = "Image size should be less than and equal to 20kb your current image size is $int_cast KB";
            
        } else if($width == $height) {
    	
            
            $response['status'] = FALSE;
            $response["message"] = "Square image not allowed";
		} else if(!(($width == 170) && ($height == 132))) {
		
           
            $response['status'] = FALSE;
            
			$response["message"] = "Image width should be 170px and height 132px your current dimensions are $width px X $height px";
		
		} else if(!(($dpi['x'] > 95 && $dpi['x'] < 201) && ($dpi['y'] > 95 && $dpi['y'] < 201))) {
			
            
            $response['status'] = FALSE;
            $response["message"] = "Image Resolution should be greater than 96 dpi and less than 200 dpi your current image horizontal resolution is ".$dpi['x']." dpi and vertical resolution is ".$dpi['y']." dpi";

		} else {


           
           
                
                $response['status'] = TRUE;
                $response["message"] = "Image validated successfully";
		

        }  
    }
	} else {

      
        $response['status'] = FALSE;
        $response["message"] = "image parameter should not be blank. Please try again!";

	}
    // echo json response
    echoRespnse(200, $response);
});

/**
 * Verifying required params posted or not
 */
function verifyRequiredParams($required_fields) {
    $error = false;
    $error_fields = "";
    $request_params = array();
    $request_params = $_REQUEST;
    // Handling PUT request params
    if ($_SERVER['REQUEST_METHOD'] == 'PUT') {
        $app = \Slim\Slim::getInstance();
        parse_str($app->request()->getBody(), $request_params);
    }
    foreach ($required_fields as $field) {
        if (!isset($request_params[$field]) || strlen(trim($request_params[$field])) <= 0) {
            $error = true;
            $error_fields .= $field . ', ';
        }
    }

    if ($error) {
        // Required field(s) are missing or empty
        // echo error json and stop the app
        $response = array();
        $app = \Slim\Slim::getInstance();
        $response["error"] = true;
        $response["message"] = 'Required field(s) ' . substr($error_fields, 0, -2) . ' is missing or empty';
        echoRespnse(400, $response);
        $app->stop();
    }
}

/**
 * Echoing json response to client
 * @param String $status_code Http response code
 * @param Int $response Json response
 */
function echoRespnse($status_code, $response) {
    $app = \Slim\Slim::getInstance();
    // Http response code
    $app->status($status_code);

    // setting response content type to json
    $app->contentType('application/json');

    echo json_encode($response);
}

$app->run();
?>