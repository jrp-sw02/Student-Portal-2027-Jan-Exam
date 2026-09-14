<?php 
class helperClass{

	function __construct() { 

		require_once __dir__.'/config.php';

    }
    public function curPageName() {
    	return substr($_SERVER["SCRIPT_NAME"], strrpos($_SERVER["SCRIPT_NAME"], "/") + 1);
	}
	public function redirect_to( $location = NULL ) 
	{
		if ($location != NULL) 
		{
			header("Location:".$location);
			exit;
		}
	}
	public function getUserIpAddr() {
    	if(!empty($_SERVER['HTTP_CLIENT_IP'])) {
        	//ip from share internet
        	$ip = $_SERVER['HTTP_CLIENT_IP'];
    	} elseif(!empty($_SERVER['HTTP_X_FORWARDED_FOR'])) {
        	//ip pass from proxy
        	$ip = $_SERVER['HTTP_X_FORWARDED_FOR'];
    	} else {
        	$ip = $_SERVER['REMOTE_ADDR'];
    	}
    	
        return $ip;
	}
	public function CheckAccess($ip) {
		
        //collection of allowed IP addresses
		$allowlist = array(
    		'127.0.0.1'
		);
		//if users IP is not in allowed list kill the script
		if(!in_array($ip,$allowlist)) {
    		return False;
		} else {
			return True;
		}

	}
	public function verifyFormToken($form) {

    	// check if a session is started and a token is transmitted, if not return an error
    	if (!isset($_SESSION[$form . '_token'])) {
        	return false;
    	}

   		 // check if the form is sent with token in it
    	if (!isset($_POST['token'])) {
        	return false;
    	}

    	// compare the tokens against each other if they are still the same
    	if ($_SESSION[$form . '_token'] !== $_POST['token']) {
       		return false;
    	}

    	return true;
	}
	public function generateFormToken($form) {

    	// generate a token from an unique value, took from microtime, you can also use salt-values, other crypting methods...
    	$token = md5(uniqid(microtime(), true));

    	// Write the generated token to the session variable to check it against the hidden field when the form is sent
    	$_SESSION[$form . '_token'] = $token;

    	return $token;
	}
    public function getRealIp() {
    if (!empty($_SERVER['HTTP_CLIENT_IP'])) {  //check ip from share internet
        $ip = $_SERVER['HTTP_CLIENT_IP'];
    } elseif (!empty($_SERVER['HTTP_X_FORWARDED_FOR'])) {  //to check ip is pass from proxy
        $ip = $_SERVER['HTTP_X_FORWARDED_FOR'];
    } else {
        $ip = $_SERVER['REMOTE_ADDR'];
    }
    return $ip;
}
    public function writeLog($where) {

    $ip = $this->getRealIp(); // Get the IP from superglobal
    $host = gethostbyaddr($ip);    // Try to locate the host of the attack
    $date = date("d M Y");

    // create a logging message with php heredoc syntax
    $logging = <<<LOG
            \n
            << Start of Message >>
            There was a hacking attempt on your form. \n 
            Date of Attack: {$date}
            IP-Adress: {$ip} \n
            Host of Attacker: {$host}
            Point of Attack: {$where}
            << End of Message >>
LOG;
// Awkward but LOG must be flush left
    // open log file
    if ($handle = fopen('hacklog.log', 'a')) {

        fputs($handle, $logging);  // write the Data to file
        fclose($handle);           // close the file
    } 
}
	
	
	
	public function ms_escape_string($data) {
        if ( !isset($data) or empty($data) ) return '';
        if ( is_numeric($data) ) return $data;

        $non_displayables = array(
            '/%0[0-8bcef]/',            // url encoded 00-08, 11, 12, 14, 15
            '/%1[0-9a-f]/',             // url encoded 16-31
            '/[\x00-\x08]/',            // 00-08
            '/\x0b/',                   // 11
            '/\x0c/',                   // 12
            '/[\x0e-\x1f]/'             // 14-31
        );
        foreach ( $non_displayables as $regex )
            $data = preg_replace( $regex, '', $data );
        $data = str_replace("'", "''", $data );
        return trim($data);
    }
    public function filter_input($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);
    return $data;
}
	
}

?> 