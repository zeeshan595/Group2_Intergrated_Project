<?php

setcookie("Username", "", time() - 3600);

header("Location: index.php");

?>