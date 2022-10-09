Write-Output "`$env:Path = `$env:Path + './node_modules/.bin;./node_modules/grpc-tools/bin;./node_modules/protoc-gen-grpc-web/bin;'"

$root = $PSScriptRoot

$env:Path = "$env:Path;$root/node_modules/.bin;$root/node_modules/grpc-tools/bin;$root/node_modules/protoc-gen-grpc-web/bin;"


protoc.exe -I ../../server/net6/src/Issues `
           --js_out=import_style=commonjs:./ `
           --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./ `
           issueTracker.issues.commands.proto

protoc.exe -I ../../server/net6/src/Issues `
           --js_out=import_style=commonjs:./ `
           --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./ `
           issueTracker.issues.queries.proto

