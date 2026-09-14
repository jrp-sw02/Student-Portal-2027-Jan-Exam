<?php

/**
 * Class to handle all db operations
 * This class will have CRUD methods for database tables
 *
 * @author Danish Akhtar
 */
class DbHandler {

    private $conn;

    function __construct() {
        require_once dirname(__FILE__) . '/dbConnect.php';
        // opening db connection
        $db = new DbConnect();
        $this->conn = $db->connect();
    }
	
	public function save($column) {
        
        $stmt = sqlsrv_query( $this->conn, "select ID from Course_Exam_Application WHERE Appl_Number = '$column[2]'", array(), array( "Scrollable" => SQLSRV_CURSOR_KEYSET ));
        if( $stmt === false ) {
            die( print_r( sqlsrv_errors(), true));
        }
        $row_count = sqlsrv_num_rows( $stmt );
          
        if ($row_count === 0)  {

            sqlsrv_free_stmt($stmt);
            RETURN FALSE;
                

        } else if ($row_count > 0) {
            
                $query = "INSERT INTO Candidate_declaration(first_name,last_name,application_number,was_declaration_accepted,declared_on,updated_on,created_on) values ('$column[0]','$column[1]','$column[2]','$column[3]',GETDATE(),GETDATE(),GETDATE())";
                $getResults = sqlsrv_query($this->conn,$query);
                $msg = TRUE;
                if ($getResults == FALSE) {
                    $msg = FALSE;
                    die( print_r( sqlsrv_errors(), true));
                } 
                sqlsrv_free_stmt($getResults);
                return $msg;
            } 
    }

    
}

?>
