Write-Output "`$env:Path = `$env:Path + './node_modules/.bin;./node_modules/grpc-tools/bin;./node_modules/protoc-gen-grpc-web/bin;'"
protoc.exe -I ../.. `
           --js_out=import_style=commonjs:./ `
           --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./ `
           issueTracker.proto
