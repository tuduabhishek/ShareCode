#!/bin/bash

# List of patterns to comment out (regex)
# Using <!-- --> for HTML/ASPX outside of server tags, and <%-- --%> if needed, 
# but most of these are in the head or script tags, so <!-- --> should work for most.
# However, for ASPX it's better to use <%-- --%> for server-side stability if they are inside <head> placeholders.
# Actually, standard <!-- --> is fine for browser-side commenting.

FILES=$(grep -lE "bootstrap.min.css|bootstrap-css|jquery.min.js|jquery.js|bootstrap.min.js" *.aspx)

for file in $FILES; do
    echo "Processing $file..."
    
    # Bootstrap CSS
    sed -i '' 's|<link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">|<!-- <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"> -->|g' "$file"
    sed -i '' 's|<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">|<!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"> -->|g' "$file"
    sed -i '' 's|<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">|<!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css"> -->|g' "$file"
    sed -i '' 's|<link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">|<!-- <link href="//netdna.bootstrapcdn.com/bootstrap/3.1.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css"> -->|g' "$file"
    
    # jQuery and jQuery UI JS
    sed -i '' 's|<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>|<!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script> -->|g' "$file"
    sed -i '' 's|<script src="https://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>|<!-- <script src="https://code.jquery.com/ui/1.10.4/jquery-ui.js"></script> -->|g' "$file"
    sed -i '' 's|<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>|<!-- <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script> -->|g' "$file"
    sed -i '' 's|<script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>|<!-- <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script> -->|g' "$file"
    sed -i '' 's|<script src="//code.jquery.com/jquery-1.11.0.min.js"></script>|<!-- <script src="//code.jquery.com/jquery-1.11.0.min.js"></script> -->|g' "$file"
    sed -i '' 's|<script src="//code.jquery.com/jquery-1.11.1.min.js"></script>|<!-- <script src="//code.jquery.com/jquery-1.11.1.min.js"></script> -->|g' "$file"
    sed -i '' 's|<script src = "https://code.jquery.com/jquery-1.10.2.js"></script>|<!-- <script src = "https://code.jquery.com/jquery-1.10.2.js"></script> -->|g' "$file"
    sed -i '' 's|<script src = "https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>|<!-- <script src = "https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script> -->|g' "$file"
    
    # Bootstrap JS
    sed -i '' 's|<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>|<!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script> -->|g' "$file"
    sed -i '' 's|<script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script>|<!-- <script src="//netdna.bootstrapcdn.com/bootstrap/3.1.0/js/bootstrap.min.js"></script> -->|g' "$file"
    sed -i '' 's|<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>|<!-- <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> -->|g' "$file"
    
    # jQuery UI CSS
    sed -i '' "s|<link href='https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/ui-lightness/jquery-ui.css' rel='stylesheet'>|<!-- <link href='https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/ui-lightness/jquery-ui.css' rel='stylesheet'> -->|g" "$file"
done

echo "Cleanup complete."
