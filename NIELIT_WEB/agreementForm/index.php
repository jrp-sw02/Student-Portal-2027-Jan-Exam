<?php
session_start();
 require_once 'includes/helper_class.php'; 
 $obj = new helperClass();
 $hack = $captcha_error = $firstNameError = $application_numberError = $application_number_not_found_Error = $check_declarationError = $successMsg = '';

if (isset($_POST["submit"])) { 
  if($_SESSION['digit'] == $_POST['captcha']) {
    if ($obj->verifyFormToken('declaration_form')) {

      $whitelist = array('firstName', 'lastName', 'application_number', 'check_declaration', 'token', 'captcha','submit');
      foreach ($_POST as $key => $item) {
        if (!in_array($key, $whitelist)) {
          $obj->writeLog('Unknown form fields');
          $hack = "Hack-Attempt detected. Please use only the fields in the form";
        }
      }
      if(empty($hack)){

        if(!empty($_POST['firstName'])) {
          $firstName = $obj->ms_escape_string($obj->filter_input($_POST["firstName"]));
        } else{
          $firstNameError = "First name is required";
        }
        if(!empty($_POST['lastName'])) {
          $lastName = $obj->ms_escape_string($obj->filter_input($_POST["lastName"]));
        } else {
          $lastName = "";
        }
        if(!empty($_POST['application_number'])) {
          $application_number = $obj->ms_escape_string($obj->filter_input($_POST["application_number"]));
        } else {
          $application_numberError = 'Application number is required';
        }
        if(!empty($_POST['check_declaration'])) {
          $check_declaration = $obj->ms_escape_string($obj->filter_input($_POST["check_declaration"]));
        } else {
          $check_declarationError = 'Please check declaration box';
        }
        if(!empty($_POST['firstName']) && !empty($_POST['application_number']) && !empty($_POST['check_declaration'])){

          require_once 'includes/dbHandler.php';
          $db = new DbHandler();
        
          $data = $db->save(array($firstName,$lastName,$application_number,$check_declaration));
          if($data) {
            $successMsg = 'Thank you for declaration now you are automatically redirect to download page after 5 second please do not refresh page.';
            header('refresh:5; url=https://student.nielit.gov.in/');

          } else {
            $application_number_not_found_Error ='Your applicaton number not found in our system.';
          }

        }
        
        
        
      }
    } else {
      $hack = "You are not authorities to use this form.";
      $obj->writeLog('declaration_form');
    }
  } else {
    $captcha_error = 'Your captcha code is wrong please try again.';
  }

}
?>

<!doctype html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="icon" href="assets/img/nielit-fevicon.jpg">

    <title>NIELIT Argeement Form</title>
    <!-- Bootstrap core CSS -->
    <link href="assets/css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom styles for this template -->
    <link href="assets/css/form-validation.css" rel="stylesheet">
  </head>

  <body class="bg-light">

    <div class="container">
      <div class="py-5 text-center">
        <img class="d-block mx-auto mb-4" src="assets/img/NIELIT-Logo.png" alt="NIELIT LOGO">
        <h2>राष्ट्रीय इलेक्ट्रॉनिकी एवं सूचना प्रौद्योगिकी संस्थान</h2>
        <h3>National Institute of Electronics & Information Technology</h3>
        <p class="lead font-weight-bold">Ministry of Electronics & Information Technology</p>
        <small class="font-weight-bold">Government of India</small>
      </div>
      <?php if(!empty($hack) || !empty($captcha_error) || !empty($application_number_not_found_Error)) { ?>
      <div class="row">
        <div class="col-md-12">
          <div class="alert alert-danger" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          <strong>Danger!</strong> <?= $hack,$captcha_error,$application_number_not_found_Error;?>
          </div>
        </div>
      </div>
      <?php } ?>
      <?php if(!empty($successMsg)) { ?>
      <div class="row">
        <div class="col-md-12">
          <div class="alert alert-success" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          <strong>Success!</strong> <?= $successMsg;?>
          </div>
        </div>
      </div>
      <?php } ?>
      <div class="row">
        <div class="col-md-12">
          <p class="mb-3">Declaration of the candidate regarding prequestionar measure is to be taken to stop spread of covid-19</p>
          <form method="POST" action="" name="declaration_form" id="declaration_form" class="needs-validation" novalidate>
            <input type="hidden" name="token" value="<?=$obj->generateFormToken('declaration_form');?>"/>
            <div class="row">
              <div class="col-md-6 mb-3">
                <label for="firstName">First name <sup class="text-danger font-weight-bold"> * </sup></label>
                <input type="text" class="form-control <?= (empty($firstNameError))?'':'is-invalid';?>" name="firstName" id="firstName" placeholder="Please enter your first name" value="<?=(isset($_POST['firstName']))?$_POST['firstName']:''?>" required>
                <div class="invalid-feedback">
                  Valid first name is required.
                </div>
                 <div class="valid-feedback">
                  Looks good!
                </div>
              </div>
              <div class="col-md-6 mb-3">
                <label for="lastName">Last name <sup class="text-danger font-weight-bold"> * </sup></label>
                <input type="text" class="form-control" name="lastName" id="lastName" placeholder="Please enter your last name" value="<?=(isset($_POST['lastName']))?$_POST['lastName']:''?>" required>
                <div class="invalid-feedback">
                  Valid last name is required.
                </div>
                 <div class="valid-feedback">
                  Looks good!
                </div>
              </div>
            </div>

            <div class="mb-3">
              <label for="application_number">Application No.<sup class="text-danger font-weight-bold"> * </sup></label>
              <input type="text" class="form-control <?= (empty($application_numberError))?'':'is-invalid';?>" name="application_number" id="application_number" placeholder="Application Number" value="<?=(isset($_POST['application_number']))?$_POST['application_number']:''?>" required>
              <div class="invalid-feedback">
                Please enter a valid application number.
              </div>
               <div class="valid-feedback">
                  Looks good!
              </div>
            </div>
            <hr class="mb-4">
            <div class="form-row">
              
                <div class="form-group col-md-12">
                  <ul>
                    <li>
                        It is compulsory to wear a face mask (preferably transparent one) and surgical gloves without which you will not be allowed entry into the exam hall. However, in case, if required you need to briefly remove the face mask to confirm your identity or to capture a photo at check-in.
                    </li>
                    <li>
                      It is mandatory to undergo thermal scanning before entering exam hall, in case of high temperature, you will not be admitted to the exam hall
                    </li>
                    <li>
                      You will not be admitted to the exam hall if you:
                      <ul>
                        <li>
                          Have been diagnosed with COVID-19 or been in close personal contact with someone with a confirmed diagnosis-
                        </li>
                        <li>
                          Have had any flu-like symptoms in the last fourteen days, including fever, chills, a cough, sore throat, shortness of breath, or loss of smell/taste-
                        </li>
                        <li>
                          Have been under fourteen day's home quarantine or centralized observations demanded by government and healthcare authorities
                        </li>
                      </ul>
                    </li>
                    <li>Wash your hands/ use hand sanitizer before entering exam hall</li>
                    <li>Stay more than 6 feet away from other people in the common areas </li>
                  </ul>
              </div>
          
          </div>
          <hr class="mb-4">
          <div class="form-group">
            <div class="form-check">
              <input class="form-check-input <?= (empty($check_declarationError))?'':'is-invalid';?>" type="checkbox" value="1" name="check_declaration" id="check_declaration" <?=(isset($_POST['check_declaration']))?'checked':''?>  required>
              <label class="form-check-label" for="check_declaration"> Declaration / घोषणा<sup class="text-danger font-weight-bold"> * </sup>  </label>
              <div class="invalid-feedback">
                You must agree before submitting.
              </div>
              <div class="valid-feedback">
                Looks good!
              </div>
            </div>
          </div>
          <div class="form-row">
           
             <div class="form-group col-md-12">
              <p>
                I, hereby declare that, I agree to abide by the rules and regulations of NIELIT and  I have noted that the Examination Authority has the right to withhold/ cancel my examination/canditature.in addition to any other action as may be deemed fit in the event of any of the statement(s) made by me in the form/above being found incorrect((मैं एतद्धारा घोषणा करता/करती हूँ कि मुझे बेसिक कम्प्यूटर कोर्स में परीक्षा आवेदन हेतु मेरी योग्यता के संबंध में रा.इ.सू.प्रौ.सं. के नियम एवं विनियम तथा परीक्षा प्राधिकारी का निर्णय मान्य है। मैं घोषणा करता/ करती हूँ कि परीक्षा फार्म में मेरे द्वारा भरी गई जानकारी मेरे ज्ञान और विश्वास के अनुसार सही हैं. मुझे सूचित है कि परीक्षा प्राधिकारी / रा.इ.सू.प्रौ.सं को मेरा परीक्षा आवेदन और परिणाम रोकने अथवा रद्द करने का अधिकार है। इसके अतिरिक्त मेरे द्वारा परीक्षा आवेदन फार्म में भरी जानकारी / उपरोक्त निर्दिष्ट कथन सही न पाए जाने पर मेरे ऊपर किसी भी प्रकार की कार्रवाई करने का अधिकार रा.इ.सू.प्रौ.सं को होगा।))
              </p>
              </div>
            
            
          </div>
          <div class="form-row">
             <div class="form-group col-md-8">
              <div class="row">
                <div class="col-md-6">
                  <label for="captcha">Enter the code from the image here:</label>
                </div>
                <div class="col-md-6">
                <input type="text" class="form-control form-control-sm" required id="captcha" name="captcha">
                <div class="invalid-feedback">
                Please enter the code from the image here
              </div>
                </div>
              </div>
              
            </div>
            <div class="form-group col-md-3 offset-md-1">
              <img src="assets/captcha/captcha.php" id="captcha_image"/>
              <a id="captcha_reload" class="btn btn-primary btn-sm" href="javascript:void(0);">Reload</a>
            </div>
            
            
           
          </div>   
      
            <hr class="mb-4">
            <div class="form-row">
              <div class="col-md-6"> 
                <a class="btn btn-primary btn-lg btn-block" href="https://student.nielit.gov.in/">
                  Return to previous page
                </a>
              </div>
              <div class="col-md-6"> 

                <button class="btn btn-primary btn-lg btn-block" name="submit" value="submit" id="submit" type="submit" <?=(isset($_POST['check_declaration']))?'':'disabled'?>>
                  Continue to Download
                </button>
              </div>
              
            </div>
           
          </form>
        </div>
        
      </div>

      <footer class="my-5 pt-5 text-muted text-center text-small">
        <p class="mb-1"> Copyright &copy; NIELIT. All Rights Reserved.</p>
        <ul class="list-inline">
          <li class="list-inline-item"><a href="https://nielit.gov.in/node/3563">Privacy</a></li>
          <li class="list-inline-item"><a href="https://nielit.gov.in/node/3561">Terms</a></li>
          <li class="list-inline-item"><a href="https://nielit.gov.in/content/help-4">Helps</a></li>
        </ul>
      </footer>
    </div>

    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="assets/js/jquery-3.5.1.min.js"></script>
    <script src="assets/js/popper.min.js"></script>
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/holder.min.js"></script>
    <script>
    window.setTimeout(function() {
           $(".alert").fadeTo(500, 0).slideUp(500, function(){
            $(this).remove(); 
           });
        }, 5000);
      // Example starter JavaScript for disabling form submissions if there are invalid fields
      (function() {
        'use strict';

        window.addEventListener('load', function() {
          // Fetch all the forms we want to apply custom Bootstrap validation styles to
          var forms = document.getElementsByClassName('needs-validation');

          // Loop over them and prevent submission
          var validation = Array.prototype.filter.call(forms, function(form) {
            form.addEventListener('submit', function(event) {
              if (form.checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
              }
              form.classList.add('was-validated');
            }, false);
          });
        }, false);
      })();
      $(function(){

        
        $('input[type="checkbox"]').click(function(){
            if($(this).is(":checked")){
                $("#submit").prop('disabled', false);
            }
            else if($(this).is(":not(:checked)")){
                $("#submit").prop('disabled', true);
            }
        });
        $('#captcha_reload').on('click',function(e){
          e.preventDefault();
          d = new Date();
          var src = $("img#captcha_image").attr("src");
          src = src.split(/[?#]/)[0];
    
          $("img#captcha_image").attr("src", src+'?'+d.getTime());
        });
      });
    </script>
  </body>
</html>

