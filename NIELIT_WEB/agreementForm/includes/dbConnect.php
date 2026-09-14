<?php

/**
 * Handling database connection
 *
 * @author Danish Akhtar
 * 
 */
class DbConnect {

    private $conn;
    private $connectionOptions;
    private $serverName;

    function __construct() { 

        include_once 'config.php';

        $this->serverName = DB_HOST;
        $this->connectionOptions = array(
        "Database" => DB_NAME,
        "Uid" => DB_USERNAME,
        "PWD" => DB_PASSWORD
        );  

    }

    /**
     * Establishing database connection
     * @return database connection handler
     */
    function connect() {
        
        //Establishes the connection
        $this->conn = sqlsrv_connect($this->serverName, $this->connectionOptions);
        

        // Check for database connection error
        if (!$this->conn) {

            echo "Connection could not be established.<br />";
            die( print_r( sqlsrv_errors(), true));
        }

        // returing connection resource
        return $this->conn;
    }

}

?>
