var uploadForm = Class.create();
uploadForm.prototype =
{
	initialize: function(fieldContainer, listContainer, statusContainer, errContainer)
	{
		this.fieldContainer = document.getElementById(fieldContainer);
		this.listContainer = document.getElementById(listContainer);
		this.statusContainer = document.getElementById(statusContainer);
		this.errContainer = document.getElementById(errContainer);
		this.statusContainer.innerHTML = 'Click Browse to select a file. Repeat to add more files.';
		this.fieldCount = 0;
		this.selectedFiles = new Array();
		this.options = new Array();
		this.setOptions({iconURL:'', fieldSize:40, allowedTypes:new Array(), publicUploader:false});
		this.zipWarning = false;
	},

	display: function()
	{
		this.addField();
	},

	setOptions: function(options)
	{
		Object.extend(this.options, options || {});
	},

	getSelectedCount: function()
	{
		return this.selectedFiles.length;
	},

	addField: function()
	{
		var field = document.createElement('input');
		field.type = 'file';
		field.name = 'file_' + this.fieldCount;
		field.size = this.options.fieldSize;
		field.onchange = this.__onchange.bind(this, field);
		this.fieldCount++;
		this.fieldContainer.appendChild(field);
	},

	showFiles: function()
	{
		this.listContainer.innerHTML = '';
		var ul = document.createElement('div');
		ul.className = 'imglist';
		for(var i = 0; i < this.selectedFiles.length; ++i)
		{
			var file = this.selectedFiles[i];
			var li = document.createElement('div');
			var leftSpan = document.createElement('span');
			var rightSpan = document.createElement('span');
			var spacer = document.createElement('div');
			spacer.className = 'spacer';
			leftSpan.className = 'fl leftimglistele';
			rightSpan.className = 'fr curhand';
			li.className = i & 1 ? 'imglistele fl imglisteleodd' : 'imglistele fl imglisteleeven';
			//li.className = 'fl imglistele';
			leftSpan.innerHTML = file.name + '&nbsp;&nbsp;';
			//rightSpan.innerHTML = '<img src="images/delete2.gif" title="Remove" alt="Remove" />';
			rightSpan.innerHTML = '<b>x</b>'
			rightSpan.onclick = this.removeFile.bind(this, i);
			li.appendChild(leftSpan);
			li.appendChild(rightSpan);
			//li.appendChild(spacer);
			//ul.appendChild(spacer);
			ul.appendChild(li);
		}
		this.listContainer.appendChild(ul);
		//ul.style.display = this.selectedFiles.length ? 'block' : 'none';
		this.statusContainer.innerHTML = this.selectedFiles.length ? this.selectedFiles.length + ' files selected.' : 'Click Browse to select a file. Repeat to add more files.';
	},

	removeFile: function(index)
	{
		this.fieldContainer.removeChild(this.selectedFiles[index].uploadField);
		this.selectedFiles.splice(index, 1);
		this.showFiles();
	},

	isSelected: function(filePath)
	{
		for(var i = 0; i < this.selectedFiles.length; ++i)
			if(this.selectedFiles[i].path == filePath) return true;
		return false;
	},

	checkExtension: function(extension)
	{
		for(var i = 0; i < this.options.allowedTypes.length; ++i)
			if(this.options.allowedTypes[i].toUpperCase() == extension.toUpperCase()) return true;
		return false;
	},

	__onchange: function(field)
	{
	    var filePath = field.value;
		if(this.isSelected(filePath))
		{
			jAlert('You have already selected ' + this.__basename(filePath),'Add Photos');
			field.value = '';
			return false;
		}
		var filename = this.__basename(filePath);
		var extension = get_extension(filename);

		/*if ( filename != 'batch.zip' && filename != 'upload.zip' )
		{
			if ( this.options.publicUploader )
			{
				if ( this.options.allowedTypes.length && !this.checkExtension(extension) )
				{
					uplAlert("You can only upload the following file types (or a Zip file named upload.zip containing these types):\n" + this.options.allowedTypes.join(', '), 'File type not allowed');
					return false;
				}
			}
			else
			{
				if ( extension == 'zip' && !this.zipWarning )
				{
					uplAlert('You have selected a Zip file. Remember to check on "Extract Zip Files" if you want to extract this file.', 'Upload tip');
					this.zipWarning = true;
				}

				if ( extension != 'zip' && this.options.allowedTypes.length && !this.checkExtension(extension) )
				{
					uplAlert("You can only upload the following file types (or a Zip file named upload.zip containing these types):\n" + this.options.allowedTypes.join(','), 'File type not allowed');
					return false;
				}
			}
		}*/
        if (!this.checkExtension(extension))
				{
					//uplAlert("You can only upload the following file types:\n" + this.options.allowedTypes.join(', '), 'File type not allowed');
					this.errContainer.innerHTML = 'Please upload only ' + this.options.allowedTypes.join(', ') + ' file types.';
					this.errContainer.style.display = '';
					//jAlert('You can only upload the following file types:\n' + this.options.allowedTypes.join(', '),'Add Photos');
					return false;
				}
		else
		    this.errContainer.style.display = 'none';		
		
		//alert(document.getElementById(this.options.existLength).value);    
		if((parseInt(document.getElementById(this.options.existLength).value) + this.getSelectedCount()) >= this.options.maxfiles)
		{
		    this.errContainer.innerHTML = 'Your limit for uploading photos is over. Please delete old photos to upload new photos.';
		    this.errContainer.style.display = '';
		    return false;
		}
		else
		    this.errContainer.style.display = 'none';
		    
		this.selectedFiles.push({name: this.__basename(filePath), path: filePath, uploadField: field});
		this.__hideField(field);
		this.addField();
		this.showFiles();
		return true;
	},

	__hideField: function(field)
	{
		field.style.position = 'absolute';
		field.style.top = '0px';
		field.style.left = '0px';
		field.style.visibility = 'hidden';
	},

	__basename: function(s)
	{
		var p=-1;for(var i=0;i<s.length;i++)if(s.charAt(i)=='\\'||s.charAt(i)=='/')p=i;if(p<0)return s;return s.substr(p+1,s.length-p);
	}
};