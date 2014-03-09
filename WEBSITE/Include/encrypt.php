<?php

function encrypt($str)
{
	$key = "a65sd46a5s7f465asdf41a6s5df4a5sd46as5d4a6s5d4a65s";
	$result = '';
	for($i=0; $i<strlen($str); $i++)
	{
		$char = substr($str, $i, 1);
		$keychar = substr($key, ($i % strlen($key))-1, 1);
		$char = chr(ord($char)+ord($keychar));
		$result.= $char;
	}
	return urlencode(base64_encode($result));
}

function decrypt($str)
{
	$str = base64_decode(urldecode($str));
	$result = '';
	$key = "a65sd46a5s7f465asdf41a6s5df4a5sd46as5d4a6s5d4a65s";
	for($i=0; $i<strlen($str); $i++)
	{
		$char = substr($str, $i, 1);
		$keychar = substr($key, ($i % strlen($key))-1, 1);
		$char = chr(ord($char)-ord($keychar));
		$result.=$char;
	}
	return $result;
}

?>